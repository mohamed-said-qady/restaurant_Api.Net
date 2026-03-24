
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

            // --- الـ 10 حالات (Exceptions) ---
            switch (exception)
            {
                case UnauthorizedAccessException: // 1. غير مصرح (Token غلط أو منتهي)
                    statusCode = HttpStatusCode.Unauthorized;
                    message = "عفواً، أنت غير مخول للقيام بهذا الإجراء.";
                    break;

                case KeyNotFoundException: // 2. عنصر غير موجود (ID غلط)
                    statusCode = HttpStatusCode.NotFound;
                    message = "العنصر المطلوب غير موجود في قاعدة البيانات.";
                    break;

                case ArgumentException: // 3. مدخلات غلط (Validation بسيط)

                    statusCode = HttpStatusCode.BadRequest;
                    message = "تأكد من صحة البيانات المرسلة.";
                    break;

                case NotImplementedException: // 4. ميثود لسه مخلصتش برمجتها
                    statusCode = HttpStatusCode.NotImplemented;
                    message = "هذه الميزة تحت التطوير حالياً.";
                    break;

                case TimeoutException: // 5. السيرفر خد وقت طويل (أو قاعدة البيانات تقيلة)
                    statusCode = HttpStatusCode.RequestTimeout;
                    message = "استغرقت العملية وقتاً طويلاً، حاول مرة أخرى.";
                    break;

                case InvalidOperationException: // 6. عملية غير منطقية (زي مسح أوردر اتمسح قبل كدة)
                    statusCode = HttpStatusCode.Conflict;
                    message = "لا يمكن إتمام العملية بسبب حالة البيانات الحالية.";
                    break;

                case System.IO.IOException: // 7. مشكلة في رفع ملف أو صورة (File System)
                    statusCode = HttpStatusCode.ServiceUnavailable;
                    message = "حدثت مشكلة أثناء التعامل مع الملفات.";
                    break;

                case Microsoft.EntityFrameworkCore.DbUpdateException: // 8. مشكلة في حفظ البيانات (Database)
                    statusCode = HttpStatusCode.BadRequest;
                    message = "فشل في تحديث البيانات، قد يكون هناك تكرار أو خطأ في الربط.";
                    break;

 

                default: // 10. أي خطأ تاني غير متوقع
                    statusCode = HttpStatusCode.InternalServerError;
                    break;
            }

            context.Response.StatusCode = (int)statusCode;
            // context.Response.ContentLength = 100;

            // --- الجزء الخاص بالأمان (Security) ---
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