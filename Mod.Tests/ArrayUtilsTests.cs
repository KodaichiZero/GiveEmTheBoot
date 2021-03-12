using GiveEmTheBoot.Utils;
using NUnit.Framework;
using System.Collections.Generic;

namespace GiveEmTheBoot.Tests {
	[TestFixture]
	public class ArrayUtilsTests {
		[Test]
		public void TestParseObjectToArray() {
			object test = new List<CommandParser.CommandSegment>
			{
				new(CommandParser.SegmentType.String, "my string"),
				new(CommandParser.SegmentType.Int, "42"),
				null,
				new(CommandParser.SegmentType.Identifier, "player")
			};

			Assert.AreEqual($"my string, 42, {ArrayUtils.NullString}, player",
				string.Join(", ", ArrayUtils.ToStringArray(test)));
		}
	}
}
