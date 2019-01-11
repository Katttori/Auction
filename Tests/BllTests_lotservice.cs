using System;
using NUnit.Framework;
using Moq;
using DAL.Interfaces;
using BLL.Services;
using DAL.Entities;
using BLL.Exceptions;

namespace Tests
{
    [TestFixture]
    public class BllTests_lotservice
    {
        //[SetUp]
        //public void LotServiceSetup()
        //{
        //    var mock = new Mock<IUnitOfWork>();
        //    mock.Setup(x => x.Products.Create(new Product { Id = 1, IsConfirmed = false }));
        //    var sud = new LotService(mock.Object);
        //}
        [Test]
        public void CreateLot_AcceptIdOfNotExistingItem_ThrowException()
        {
            //assign
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(x => x.Products.Create(new Product { Id = 1 }));
            var sud = new LotService(mock.Object);
            //act

            //assert
            Assert.Throws(typeof(NotFoundException), () => sud.CreateLot(2));
        }
        [Test]
        public void CreateLot_AcceptIdOfNotConfirmedItem_ThrowException()
        {
            //assign
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(x => x.Products.Get(1)).Returns(new Product { Id = 1, IsConfirmed = false});
            var sud = new LotService(mock.Object);
            //act
            var message = Assert.Catch(typeof(InvalidOperationException), () => sud.CreateLot(1)).Message;
            //assert
            Assert.AreEqual("Not confirmed", message);
        }
    }
}
