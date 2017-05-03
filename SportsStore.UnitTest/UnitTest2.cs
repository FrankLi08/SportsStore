using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Web.Mvc;
using SportsStore.Controllers;
using SportStore.Domain.Abstract;
using SportStore.Domain.Entities;
using System.Linq;
using System.Collections.Generic;
using SportsStore.Models;
using SportsStore.HTMLHelper;

namespace SportsStore.UnitTest
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void Can_Filter_Products()
        {
            // Arrange
            // - create the mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
new Product {ProductID = 1, Name = "P1", Category = "Cat1"},
new Product {ProductID = 2, Name = "P2", Category = "Cat2"},
new Product {ProductID = 3, Name = "P3", Category = "Cat1"},
new Product {ProductID = 4, Name = "P4", Category = "Cat2"},
new Product {ProductID = 5, Name = "P5", Category = "Cat3"}
});
            // Arrange - create a controller and make the page size 3 items
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;
            // Action
            Product[] result = ((ProductsListViewModel)controller.List("Cat2", 1).Model)
            .Products.ToArray();
            // Assert
            Assert.AreEqual(result.Length, 2);
            Assert.IsTrue(result[0].Name == "P2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name == "P4" && result[1].Category == "Cat2");
        }

        [TestMethod]
        public void Can_Create_Categories()
        {
            // Arrange
            // - create the mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
new Product {ProductID = 1, Name = "P1", Category = "Apples"},
new Product {ProductID = 2, Name = "P2", Category = "Apples"},
new Product {ProductID = 3, Name = "P3", Category = "Plums"},
new Product {ProductID = 4, Name = "P4", Category = "Oranges"},
});
            // Arrange - create the controller
            NavController target = new NavController(mock.Object);
            // Act = get the set of categories
            string[] results = ((IEnumerable<string>)target.Menu().Model).ToArray();
            // Assert
            Assert.AreEqual(results.Length, 3);
            Assert.AreEqual(results[0], "Apples");
            Assert.AreEqual(results[1], "Oranges");
            Assert.AreEqual(results[2], "Plums");
        }

        [TestMethod]
        public void Cannot_Checkout_Empty_Cart()
        {
            //arrange create a mock order processor
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            // arrange create an empty cart
            Cart cart = new Cart();
            //arrange create shiupping details
            ShippingDetails shippingDetails = new ShippingDetails();
            //arrange create an instance of the controller
            CartController target = new CartController(null, mock.Object);

            //act
            ViewResult result = target.Checkout(cart, shippingDetails);

            //assert check that the order hasn't been passed ont to the processor
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never());
            //question?
            //assert check that the method is returning  the default view
            Assert.AreEqual("", result.ViewName);
            //asser check that I am passing an invalid model to the view
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            //arrange create a mock order processor
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            //arrage create a cart with an item
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);

            //arragne create an instance of the controller
            CartController target = new CartController(null, mock.Object);
            //arragnge add anerror to the model
            target.ModelState.AddModelError("error", "error");

            //act try to checkout
            ViewResult result = target.Checkout(cart, new ShippingDetails());

            //assert check that the order hasn't been passed on tho the processor
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never());
            //assert check that i am passin ginvalid model to the view 
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);

        }

        [TestMethod]
        public void Can_Checkout_and_submit_order()
        {
            // arrange create a mock 
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            //arrange create a cart with item
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);
            //arragne create an instance o fthe controller
            CartController target = new CartController(null,mock.Object);

            //act try to checkout
            ViewResult result = target.Checkout(cart, new ShippingDetails());

            //assert check  that the order has been passed on to the processor
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Once());
            //assert check that the method is returning the completed view
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);

        }
        
    }
}
