using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ValuesController : ControllerBase
    {
        [HttpPost]
        public IEnumerable<TestDto> Post([FromBody] IEnumerable<TestDto> dtos)
        {
            return dtos;
        }

        [HttpGet]
        [HttpPost]
        public IList<TestDto> Test(int quantity)
        {
            return CreateDtos(quantity);
        }

        [HttpGet]
        [HttpPost]
        public TestDto TestSimple()
        {
            return CreateDtos(1).First();
        }

        private static IList<TestDto> CreateDtos(int quantity)
        {
            return Enumerable.Range(0, quantity <= 0 ? 10 : quantity).Select(p => new TestDto
            {
                Id = Guid.NewGuid(),
                CreateTime = DateTime.Now,
                Enum = TestEnum.Apple,
                Name = Guid.NewGuid().ToString()
            }).ToList();
        }
    }
}