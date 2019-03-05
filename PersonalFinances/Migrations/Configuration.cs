namespace PersonalFinances.Migrations
{
    using System.Data.Entity.Migrations;

    using Models;

    internal sealed class Configuration : DbMigrationsConfiguration<Repositories.DatabaseContext>
    {
        private readonly bool _seedDatabase = true;

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Repositories.DatabaseContext context)
        {
            if (_seedDatabase)
            {
                // Add Transfer category and subcategory
                Subcategory transferCreditSubcategory = new Subcategory
                {
                    Name = "Other",
                    Category = new Category { Name = "Transfer", Type = "C", Enabled = true },
                    Enabled = true,
                    CanEdit = false
                };

                Subcategory transferDebitSubcategory = new Subcategory
                {
                    Name = "Other",
                    Category = new Category { Name = "Transfer", Type = "D", Enabled = true },
                    Enabled = true,
                    CanEdit = false
                };

                // Add Payment category and Credit card subcategory
                Subcategory creditCardSubcategory = new Subcategory
                {
                    Name = "Credit card",
                    Category = new Category { Name = "Payments", Type = "D", Enabled = true },
                    Enabled = true,
                    CanEdit = false
                };

                context.Subcategories.AddOrUpdate(
                    transferCreditSubcategory, 
                    transferDebitSubcategory, 
                    creditCardSubcategory
                );

                context.SaveChanges();
            }
        }
    }
}
