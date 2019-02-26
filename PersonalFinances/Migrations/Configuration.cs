namespace PersonalFinances.Migrations
{
    using System.Data.Entity.Migrations;

    using Models;

    internal sealed class Configuration : DbMigrationsConfiguration<PersonalFinances.Repositories.DatabaseContext>
    {
        private readonly bool _seedDatabase = false;

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(PersonalFinances.Repositories.DatabaseContext context)
        {
            if (_seedDatabase)
            {
                // Add Transfer category and subcategory
                Subcategory creditSubcategory = new Subcategory
                {
                    Name = "Other",
                    Category = new Category { Name = "Transfer", Type = "C", Enabled = true },
                    Enabled = true
                };

                Subcategory debitSubcategory = new Subcategory
                {
                    Name = "Other",
                    Category = new Category { Name = "Transfer", Type = "D", Enabled = true },
                    Enabled = true
                };

                context.Subcategories.Add(creditSubcategory);
                context.Subcategories.Add(debitSubcategory);
                context.SaveChanges();
            }
        }
    }
}
