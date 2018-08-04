using System;
using System.Collections.Generic;
using ProtoBuf;

namespace Demo
{
    [ProtoContract]
    public class TestDto
    {
        [ProtoMember(1)] public Guid Id { get; set; }
        [ProtoMember(2)] public string Name { get; set; }
        [ProtoMember(3)] public DateTime CreateTime { get; set; }
        [ProtoMember(4)] public List<TestDto> Kids { get; set; }
        [ProtoMember(5)] public long Tag { get; set; }
        [ProtoMember(6)] public TestEnum Enum { get; set; }
    }
}