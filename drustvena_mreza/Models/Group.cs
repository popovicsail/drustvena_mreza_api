namespace drustvena_mreza.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfCreation { get; set; }
        public List<User> GroupUsers { get; set; } = new List<User>();

        public Group(int id, string name, DateTime dateOfCreation)
        {
            Id = id;
            Name = name;
            DateOfCreation = dateOfCreation;
        }
        public string FormatZaSave()
        {
            return $"{Id},{Name},{DateOfCreation}";
        }

    }
}
