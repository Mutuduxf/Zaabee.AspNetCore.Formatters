using System;
using System.Collections.Generic;

namespace Demo
{
    public class TestDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }
        public List<TestDto> Kids { get; set; }
        public long Tag { get; set; }
        public TestEnum Enum { get; set; }
//        [ProtoMember(7)] public DateTimeOffset TestTime { get; set; }
    }
}