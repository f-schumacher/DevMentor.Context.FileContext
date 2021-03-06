DevMentor.Context.FileContext
=============================

![DevMentor Logo](http://devmentor.de/templates/devmentor/images/devmentor_logo.png "DevMentor")

FileContext is a data access layer (DAL) Framework for 
Rapid Data Driven Application Development (RDDAD). 

###benefits
  > 1. you don't need a database
  > 2. rapid data-modelling, -modification
  > 3. share your informations with version-control
  > 4. quick synchronisation between environments (test -> dev)
  > 5. all serializable .NET types are allowed (DateTime.Min, TimeSpan > 24h,...)
  > 6. all linq queries with extension-methods as FilterExpression are allowed
  > 7. rapid migration to Entity-Framework 6.*
  > 8. Generic repository pattern included
  > 9. Unit of work pattern included
  >10. Threadsafe for prototyping
  >11. Different store strategies (Xml,Json,InMemory,MongoDb,Cache)
  >12. Caching as store wrapper
  >13. Support transactions
  
hint: don't use for production.

###nuget
> #### PM> install-package filecontext 
> #### PM> install-package filecontext.mongodb 

###Todo: in two steps to FileContext
modify EntityFramework DbContext to FileContext
  >1. Replace DbContext to FileContext
  >2. Replace DbSet to FileSet

###Example Code:
 
```C#
var unit = new UnitOfWork(new Context()); //new Context(new InMemoryStoreStrategy())
//INSERT
Console.WriteLine("INSERT PAUL");
unit.UserRepository.Insert(new User() { UserName="pmizel",
                                        FirstName="Paul", 
                                        LastName="Mizel",
                                        Birthday=new DateTime(1980,4,1)});
Console.WriteLine("INSERT FABIAN");
unit.UserRepository.Insert(new User() { UserName = "fraetz", 
                                        FirstName = "Fabian",
                                        LastName = "Raetz",
                                        Birthday = new DateTime(1989, 4, 1)});
unit.Save();

Console.WriteLine("GET ALL");
var users=unit.UserRepository.Get(); //GET ALL
foreach (var user in users)
{
    Console.WriteLine("{0} {1} ({2})", user.FirstName, user.LastName, user.UserName);
}

Console.WriteLine("GET Youngest");
var youngest = unit.UserRepository.Get(orderBy: o => o.OrderByDescending(i => i.Birthday)).First(); 
Console.WriteLine("{0} {1} ({2}) - {3:dd.MM.yyyy}", youngest.FirstName, 
                                                    youngest.LastName, 
                                                    youngest.UserName,
                                                    youngest.Birthday);

Console.WriteLine("GET PAULs");
users = unit.UserRepository.Get(f=>f.FirstName=="Paul"); //GET ALL
foreach (var user in users)
{
    Console.WriteLine("{0} {1} ({2})", user.FirstName, user.LastName, user.UserName);
}
Console.WriteLine("UPDATE PAULs to PABLOs");
foreach (var user in users)
{
    user.FirstName = "Pablo";
    unit.UserRepository.Update(user);
}
unit.Save();

Console.WriteLine("GET ALL");
users = unit.UserRepository.Get(); //GET ALL
foreach (var user in users)
{
    Console.WriteLine("{0} {1} ({2})", user.FirstName, user.LastName, user.UserName);
}

//DELETE ALL
Console.WriteLine("DELETE ALL");
unit.UserRepository.Delete(unit.UserRepository.Get());
users = unit.UserRepository.Get();
foreach (var user in users)
{
    Console.WriteLine("{0} {1} ({2})", user.FirstName, user.LastName, user.UserName);
}
unit.Save();
```

###Output:
```sh
INSERT PAUL
INSERT FABIAN
GET ALL
Paul Mizel (pmizel)
Fabian Raetz (fraetz)
GET Youngest
Fabian Raetz (fraetz) - 01.04.1989
GET PAULs
Paul Mizel (pmizel)
UPDATE PAULs to PABLOs
GET ALL
Fabian Raetz (fraetz)
Pablo Mizel (pmizel)
DELETE ALL
```

###Example Context: 

```C#
//public class ContentItemContext : DbContext
public class ContentItemContext : FileContext
{
	public ContentItemContext()
		: base("name=ContentItemContext")
	{
	}

	public FileSet<ContentItem> ContentItems { get; set; }
	//public DbSet<ContentItem> ContentItems { get; set; }
}
```


###StoreStrategy

```C#
//public class ContentItemContext : DbContext
public class ContentItemContext : FileContext
{
	public ContentItemContext()
		: base(new JsonStoreStrategy()) 
		// or DefaultStoreStrategy() => XmlStoreStrategy()
		// or InMemoryStoreStrategy()
	{
	}

	public FileSet<ContentItem> ContentItems { get; set; }
	//public DbSet<ContentItem> ContentItems { get; set; }
}
```

for UnitTests/IntegrationsTests use InMemoryStoreStrategy instance.
