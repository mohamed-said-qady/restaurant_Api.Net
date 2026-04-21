using Moq; // مهم جداً
using Xunit;
using restaurant.Dtos;
using system;
using restaurant.Repositories.Interfaces;
using restaurant.Services; // تأكد من عمل using لمكان الـ OrderService
using restaurant.Model; // عشان يشوف كلاس الـ Inventory والـ Order

namespace Restaurant.UnitTests.Services
{
    public class OrderServiceTests
    {
        // 1. بنعرف الـ Mock والـ Service كـ private fields
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
                OrderDetails = new List<OrderDetailDto> {
                    new OrderDetailDto { MenuItemId = Guid.NewGuid(), Quantity = 1 }
                }
            };

            // بنجهز الـ Mock للـ InventoryRepository والـ Transaction
            // لأن كودك بيبدأ بـ BeginTransactionAsync فلازم الـ Mock يعرف يتصرف لما يشوفها
            _unitOfWorkMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.RollbackTransactionAsync()).Returns(Task.CompletedTask);

            // بنحاكي إن الصنف مش موجود في المخزن
            _unitOfWorkMock.Setup(u => u.Inventory.GetByMenuItemIdAsync(It.IsAny<Guid>()))
                           .ReturnsAsync((Inventory)null);

            // Act
            var result = await _orderService.CreateAsync(userId, dto);

            // Assert
            Assert.False(result.IsSuccess); // اتأكد من اسم الخاصية عندك هل هي IsSuccess ولا IsSucceeded
            Assert.Equal("الصنف غير موجود او تم حذفه ", result.Message);

            // التأكد إن الـ Rollback اتنادى مرة واحدة فعلاً
            _unitOfWorkMock.Verify(u => u.RollbackTransactionAsync(), Times.Once);
        }
    }
}