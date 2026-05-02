using Microsoft.Identity.Client;
using Moq; // مهم جداً
using restaurant.Dtos;
using restaurant.Model; // عشان يشوف كلاس الـ Inventory والـ Order
using restaurant.Repositories.Interfaces;
using restaurant.Services; // تأكد من عمل using لمكان الـ OrderService
using restaurant.Services.Interfaces;
using system;
using System.Collections.Generic;
using Xunit;
using restaurant.Data;
using restaurant.Data.Enums;

namespace Restaurant.UnitTests.Services
{
    public class OrderServiceTests
    {
        //1. بنعرف الـ Mock والـ Service كـ private fields
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            // 2. بنكريت الـ Mock يدويًا
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            // 3. بنعمل Instance من السيرفس الحقيقية ونبعتلها الـ Mock Object
            _orderService = new OrderService(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnFailure_WhenItemNotFoundInInventory()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var dto = new OrderCreateDto
            {
                OrderDetails = new List<OrderItemCreateDto> {
                    new OrderItemCreateDto { MenuItemId = 1 , Quantity = 1 }
                }
            };

            // بنجهز الـ Mock للـ InventoryRepository والـ Transaction
            // لأن كودك بيبدأ بـ BeginTransactionAsync فلازم الـ Mock يعرف يتصرف لما يشوفها
            _unitOfWorkMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.RollbackTransactionAsync()).Returns(Task.CompletedTask);

            // بنحاكي إن الصنف مش موجود في المخزن
            _unitOfWorkMock.Setup(u => u.Inventory.GetByMenuItemIdAsync(dto.OrderDetails[0].MenuItemId)).ReturnsAsync((InventoryItem?)null);

            // Act
            var result = await _orderService.CreateAsync(userId, dto);

            // Assert
            Assert.False(result.IsSuccess); // اتأكد من اسم الخاصية عندك هل هي IsSuccess ولا IsSucceeded
            Assert.Equal("الصنف غير موجود او تم حذفه ", result.Message);

            // التأكد إن الـ Rollback اتنادى مرة واحدة فعلاً
            _unitOfWorkMock.Verify(u => u.RollbackTransactionAsync(), Times.Once);
        }

        //TEST FOR QUANTITY 
        [Fact]
        public async Task ShouldReturnFalseWhenQuantityNotFound()
        {
            // 1. Arrange (التجهيز)
            var userId = Guid.NewGuid();
            var dto = new OrderCreateDto
            {
                DeliveryAddress = "Cairo",
                OrderDetails = new List<OrderItemCreateDto>
        {
            new OrderItemCreateDto { MenuItemId = 1, Quantity = 5 } // طالب 5
        }
            };

            // بنجهز صنف في المخزن كميته 2 بس (أقل من الـ 5 اللي في الـ DTO)
            var fakeInventory = new InventoryItem
            {
                MenuItemId = 1,
                Quantity = 2, // المخزن فيه 2 بس
                MenuItem = new MenuItem { Price = 100 }
            };

            _unitOfWorkMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.RollbackTransactionAsync()).Returns(Task.CompletedTask);

            // لما السيرفس تسأل عن الصنف رقم 1، الموك هيرجع الـ fakeInventory اللي كميته 2
            _unitOfWorkMock.Setup(u => u.Inventory.GetByMenuItemIdAsync(1))
                           .ReturnsAsync(fakeInventory);

            // 2. Act (التنفيذ)
            var result = await _orderService.CreateAsync(userId, dto);

            // 3. Assert (التحقق)
            Assert.False(result.IsSuccess);
            Assert.Equal("الكمية غير كافية", result.Message);

            // أهم حاجة: نتأكد إنه عمل Rollback لما لقى الكمية مش كفاية
            _unitOfWorkMock.Verify(u => u.RollbackTransactionAsync(), Times.Once);
        }
        [Fact]
        public async Task UpdateOrder_OrderNotExistReternFalse()
        {
            var OrderDataFake = new OrderUpdateDto
            {
                CustomerNotes = "الطلب  ياتئ سخن رجاءا",
                DeliveryAddress = "cairo"
            };

            _unitOfWorkMock.Setup(u => u.Orders.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Order)null!);
            var result = await _orderService.UpdateAsync(1, OrderDataFake);
            Assert.Equal("عفو هذا الاوردر غير موجود", result.Message);
        }
        [Fact]
        public async Task UpdateOrder_OrderExistReturn()
        {
            var OrderData = new OrderUpdateDto()
            {
                CustomerNotes = "الرجاء التوصيل سريعا",
                DeliveryAddress = "الجيزه حي الدقي ش 125 ",
                Status = OrderStatus.Pending
            };
            // هنا بنعمل أوردر وهمي موجود فعلاً في الداتا بيز
            var existingOrder = new Order { Id = 1, CustomerNotes = "Old Note" };
            _unitOfWorkMock.Setup(u => u.Orders.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((existingOrder));
            _unitOfWorkMock.Setup(u => u.CompleteAsync())
                   .ReturnsAsync(1);
            var result = await _orderService.UpdateAsync(1, OrderData);
            Assert.Equal("تم تعديل الاوردر بنجاح", result.Message);

        }
        [Fact]
        public async Task UpdateOrder_OrderStatusIsCanceledReturnFailure()
        {
            var OrderDataDto = new OrderUpdateDto()
            {
                Status = OrderStatus.Cancelled
            };
            var OrderData = new Order()
            {
                Status = OrderStatus.Cancelled
            };
            _unitOfWorkMock.Setup(u => u.Orders.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((OrderData));
            var result = await _orderService.UpdateAsync(1, OrderDataDto);
            Assert.Equal("لا يمكن تعديل طلب تم توصيله أو إلغاؤه", result.Message);

        }


    }
}



