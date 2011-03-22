using Expedia;
using System;

namespace ExpediaTest
{
	public class ObjectMother
	{
        public static Car BMW()
        {
            return new Car(10) { Name = "BMW" };
        }
	}
}
