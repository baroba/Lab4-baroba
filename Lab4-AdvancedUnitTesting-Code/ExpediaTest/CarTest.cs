using System;
using NUnit.Framework;
using Expedia;
using Rhino.Mocks;
using System.Reflection; 
namespace ExpediaTest
{
	[TestFixture()]
	public class CarTest
	{	
		private Car targetCar;
		private MockRepository mocks;
		
		[SetUp()]
		public void SetUp()
		{
			targetCar = new Car(5);
			mocks = new MockRepository();
		}
		
		[Test()]
		public void TestThatCarInitializes()
		{
			Assert.IsNotNull(targetCar);
		}	
		
		[Test()]
		public void TestThatCarHasCorrectBasePriceForFiveDays()
		{
			Assert.AreEqual(50, targetCar.getBasePrice()	);
		}
		
		[Test()]
		public void TestThatCarHasCorrectBasePriceForTenDays()
		{
            var target = ObjectMother.BMW(); 
			Assert.AreEqual(80, target.getBasePrice());	
		}
		
		[Test()]
		public void TestThatCarHasCorrectBasePriceForSevenDays()
		{
			var target = new Car(7);
			Assert.AreEqual(10*7*.8, target.getBasePrice());
		}
		
		[Test()]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestThatCarThrowsOnBadLength()
		{
			new Car(-5);
		}
        [Test()]
        public void TestThatCarDoesGetLocationFromTheDatabase()
        {
            IDatabase mockDatabase = mocks.Stub<IDatabase>();
            String carLocation = "Whale Rider";
            String carLocation2 = "Raptor Wrangler";
            using(mocks.Record())
            {
                mockDatabase.getCarLocation(24);
                LastCall.Return(carLocation);
                mockDatabase.getCarLocation(1025);
                LastCall.Return(carLocation2);
            }
            var target = new Car(10);
            target.Database = mockDatabase;
            String result;
            result = target.getCarLocation(1025);
            Assert.AreEqual(result, carLocation2);
            result = target.getCarLocation(24);
            Assert.AreEqual(result, carLocation);
        }
        [Test()]
        public void TestThatHotelDoesGetRoomCountFromDatabase()
        {
            IDatabase mockDatabase = mocks.Stub<IDatabase>();
            int Miles = 42; 
            mockDatabase.Miles = Miles;
            var target = new Car(10);
            target.Database = mockDatabase;
            var m = target.Database.Miles;
            Assert.AreEqual(m,Miles);
        }

        [Test()]
        public void TestThatUserDoesRemoveCarFromServiceLocatorWhenBooked()
        {
            ServiceLocator serviceLocator = new ServiceLocator();
            var carToBook = new Car(5);
            var remainingCar = new Car(7);
            serviceLocator.AddCar(carToBook);
            serviceLocator.AddCar(remainingCar);
            typeof(ServiceLocator).GetField("_instance", BindingFlags.Static | BindingFlags.NonPublic)
            .SetValue(serviceLocator, serviceLocator);
            var target = new User("Bob");
            target.book(carToBook);
            Assert.AreEqual(1, ServiceLocator.Instance.AvailableCars.Count);
            Assert.AreSame(remainingCar, ServiceLocator.Instance.AvailableCars[0]);
        }
	
   }
}
