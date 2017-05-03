using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportStore.Domain.Abstract;
using SportStore.Domain.Entities;
using SportsStore.Controllers;
using System.Web.Mvc;


namespace SportsStore.UnitTest
{
    [TestClass]
    class AdminTest
    {
        [TestMethod]
        public void Index_Cotains_all_product()
        {
            //arrange create the mock
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
            new Product { ProductID=1, Name="p1"}, new Product { ProductID=2, Name="p2"}, new Product { ProductID=3, Name="p3"}, });
            //arrange create a controller
            AdminController target = new AdminController(mock.Object);
            //action
            Product[] result = ((IEnumerable<Product>)target.Index().ViewData.Model).ToArray();
            //assert
            Assert.AreEqual(result.Length, 3);
            Assert.AreEqual("p1", result[0].Name);
            Assert.AreEqual("p2", result[1].Name);
            Assert.AreEqual("p3", result[2].Name);
            // can not be test

        }

        //[TestMethod]
        //public void Can_edit_product()
        //{
        //    //arrage create the mock repository
        //    Mock<IProductRepository> mock = new Mock<IProductRepository>();
        //    mock.Setup(m => m.Products).Returns(

        //        new Product[] {
        //            new Product { ProductID=1, Name="P1"},
        //            new Product { ProductID=2, Name="P2"},
        //            new Product { ProductID=3, Name="P3"}
        //        });
        //    //arrange create controller 
        //    AdminController target = new AdminController(mock.Object);
        //    //Act
        //    Product p1 = target.Edit(1).ViewData.Model as Product;
        //    Product p2 = target.Edit(2).ViewData.Model as Product;
        //    Product p3 = target.Edit(3).ViewData.Model as Product;
        //    //what is the meaning of this 
        //    //assert
        //    Assert.AreEqual(1, p1.ProductID);
        //    Assert.AreEqual(2, p2.ProductID);
        //    Assert.AreEqual(3, p3.ProductID);
        //}
        [TestMethod]
        public void Cannot_Edit_Nonexistent_Product()
        {
            //arrage create the mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

        }
        [TestMethod]
        public void Can_Save_Valid_Change()
        {
            //arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            //arrange create contro;;er
            AdminController target = new AdminController(mock.Object);
            //create a product
            Product product = new Product { Name = "Test" };

            //act - try to save the product
            ActionResult result = target.Edit(product);
            //assert check that the repository was called 
            mock.Verify(m => m.SaveProduct(product));
            //assert check the method result type
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));



        }

    }
}
