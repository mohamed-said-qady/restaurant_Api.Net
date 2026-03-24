
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting; // مهم عشان IWebHostEnvironment

namespace restaurant.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;

        public GlobalExceptionMiddleware(RequestDelegate next, IWebHostEnvironment env)
        {
            _next = next;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
           

            var statusCode = HttpStatusCode.InternalServerError;
            var message = "حدث خطأ غير متوقع في السيرفر.";


            switch (exception)
            {
                case UnauthorizedAccessException: 
                    statusCode = HttpStatusCode.Unauthorized;
                    message = "عفواً، أنت غير مخول للقيام بهذا الإجراء.";
                    break;

                case KeyNotFoundException: 
                    statusCode = HttpStatusCode.NotFound;
                    message = "العنصر المطلوب غير موجود في قاعدة البيانات.";
                    break;

                case ArgumentException: 

                    statusCode = HttpStatusCode.BadRequest;
                    message = "تأكد من صحة البيانات المرسلة.";
                    break;

                case NotImplementedException: 
                    statusCode = HttpStatusCode.NotImplemented;
                    message = "هذه الميزة تحت التطوير حالياً.";
                    break;

                case TimeoutException: 
                    statusCode = HttpStatusCode.RequestTimeout;
                    message = "استغرقت العملية وقتاً طويلاً، حاول مرة أخرى.";
                    break;

                case InvalidOperationException: 
                    statusCode = HttpStatusCode.Conflict;
                    message = "لا يمكن إتمام العملية بسبب حالة البيانات الحالية.";
                    break;

                case System.IO.IOException:
                    statusCode = HttpStatusCode.ServiceUnavailable;
                    message = "حدثت مشكلة أثناء التعامل مع الملفات.";
                    break;

                case Microsoft.EntityFrameworkCore.DbUpdateException: 
                    statusCode = HttpStatusCode.BadRequest;
                    message = "فشل في تحديث البيانات، قد يكون هناك تكرار أو خطأ في الربط.";
                    break;

 

                default: 
                    statusCode = HttpStatusCode.InternalServerError;
                    break;
            }

            context.Response.StatusCode = (int)statusCode;
            // context.Response.ContentLength = 100;

            
            var response = new
            {
                status = context.Response.StatusCode,
                message = message,
                // لو إنت على جهازك (Development) اظهر التفاصيل، لو Production اخفيها
                details = _env.IsDevelopment() ? exception.Message : "يرجى التواصل مع الدعم الفني.",
                innerException = _env.IsDevelopment() ? exception.InnerException?.Message : null,
                stackTrace = _env.IsDevelopment() ? exception.StackTrace : null // بيعرفك السطر بالظبط
            };

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
        }
    }
}