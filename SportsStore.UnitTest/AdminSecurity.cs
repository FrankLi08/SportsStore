using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Infrastructure.Abstract;
using SportsStore.Models;
using SportStore.Domain.Abstract;
using System.Linq;
using SportStore.Domain.Entities;

namespace SportsStore.UnitTest
{
    [TestClass]
    public class AdminSecurity
    {
        [TestMethod]
        public void Can_Login_With_Valid_Credentials()
        {
            //arrang create a mock authentication provdier
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("admin", "secret")).Returns(true);
            //arrange create the view model
            //
            LoginViewModel model = new LoginViewModel
            {
                UserName = "admin",
                Password = "secret"

            };
            //arragne create the controller
            AccountController target = new AccountController(mock.Object);
            //act  authenticate using valid credentials
            ActionResult result = target.Login(model, "/MyURL");

            //Assert
            Assert.IsInstanceOfType(result, typeof(RedirectResult));
            Assert.AreEqual("/MyURL", ((RedirectResult)result).Url);
        }

        [TestMethod]
        public void Cannot_Login_with_invalid_credentials()
        {
            //arrange create a mock authentication provider
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("badUser", "badpass")).Returns(false);

            //arrange create the view model
            LoginViewModel model = new LoginViewModel
            {
                UserName = "baduser",
                Password = "badpass"
            };
            //arrange create controlller
            AccountController target = new AccountController(mock.Object);

            //act authenticate using valid credentials
            ActionResult result = target.Login(model, "/MyURL");

            //Assert 
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsFalse(((ViewResult)result).ViewData.ModelState.IsValid);
            //what is the meaning of this
        }

        [TestMethod]
        public void Can_Retrieve_iamge_data()
        {
            //arragne cerate a product with image data
            Product prod = new Product
            {
                ProductID = 2,
                Name = "Test",
                ImageData = new byte[] { },
                ImageMimeType = "Image/png"
            };

            //arrange create mock objecet
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product { ProductID=1, Name="P1"}, prod,
                new Product { ProductID=3, Name="P3"}
            }.AsQueryable());


            // arragne create control
            ProductController target = new ProductController(mock.Object);
            //Act call the getimage action method
            ActionResult result = target.GetImage(2);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreEqual(prod.ImageMimeType, ((FileResult)result).ContentType);
        }
        [TestMethod]
        public void Cannot_Retrieve_Image_Data_For_Invalid_ID()
        {
            //arrange create the mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product { ProductID=1, Name="P1"},
                new Product { ProductID=2, Name="P2"}
            }.AsQueryable());
            //arrange  cerate the controller 
            ProductController target = new ProductController(mock.Object);

            //Act call the getiamge action mehtod
            ActionResult result = target.GetImage(100);
            //assert
            Assert.IsNull(result);
        }
    }
}
