using Microsoft.Identity.Client;
using Moq; // مهم جداً
using restaurant.Data;
using restaurant.Data.Enums;
using restaurant.Dtos;
using restaurant.Model; // عشان يشوف كلاس الـ Inventory والـ Order
using restaurant.Repositories.Implementations;
using restaurant.Repositories.Interfaces;
using restaurant.Services;
using restaurant.Services.Implementations;
using restaurant.Services.Interfaces;
using system;
using System.Collections.Generic;
using Xunit;

namespace Restaurant.UnitTests_.Services
{
    public class InventoryServiceTests
    {
        private IInventoryService _InventoryService;
        private Mock<IUnitOfWork> _UnitOfWorkMock;
        public InventoryServiceTests()
        {
            _UnitOfWorkMock = new Mock<IUnitOfWork>();
            _InventoryService = new InventoryService(_UnitOfWorkMock.Object);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnFailure_WhenInventoryAlreadyExists()
        {
            var InventoryCreateDto = new InventoryCreateDto
            {
                MenuItemId = 1,
                Quantity = 3
            };
            var existingInventory = new InventoryItem { Id = 1, MenuItemId = 1, Quantity = 0 };
            _UnitOfWorkMock.Setup(u => u.Inventory.GetByMenuItemIdAsync(It.IsAny<int>())).ReturnsAsync(existingInventory);
            var result = await _InventoryService.CreateAsync(InventoryCreateDto);
            Assert.False(result.IsSuccess);
            Assert.Equal("هذا المخزون موجود بالفعل", result.Message);

        }
    }
}
