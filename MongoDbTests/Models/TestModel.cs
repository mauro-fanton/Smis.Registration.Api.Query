    using Mongo.Migration.Documents;

namespace MongoDbTests.Models
{
    public class TestModel
	{

		public TestModel(string name, DateTime dob, string occupation)
		{
            Name = name;
            Dob = dob;
            Occupation = occupation;
        }

        public DocumentVersion Version { get; set; }
        public string Name { get; }
        public DateTime Dob { get; }
        public string Occupation { get; }
    }
}

