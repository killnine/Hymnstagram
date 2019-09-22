# Hymnstogram #

## Types and Relationships ##

### Types ###

* Song: Songs have a title, creators, key, solfa starting note, time signature, song number in their songbook, and tunes.
* Songbook: A songbook contains songs. Songbooks have titles, publishers, ISBNs, creators, and songs.
* Creator: A creative contributor to either a songbook or song. They have a first and last name, a type of creative they're associated with, and a type of resource they're associated with (Song or Songbook)


### Relationship Map: Option 1 ###

* Songs have 1:N Creators
* Songbooks have 1:N Creators
* Songbooks have 1:N Songs

This means that Creators may be duplicated across songs and songbooks, with their own CreativeType and ResourceType fields.

### Relationship Map: Option 2 ###

* Creators have 1:N CreativeTypes
* Creators have 1:N ResourceTypes
* Songs have N:M Creators
* Songbooks have 1:N Creators
* Songbooks have 1:N Songs

Creators: Stores people (First Name, Last Name)
CreativeTypeResourceTypeXref: Maps CreativeTypes (Writer, Composer, Editor, Technical Editor, etc.) to a Specific Resource (Song, Songbook)
CreatorCreativeTypeXref: Maps Creators to a specific CreativeType

This means that Creators are more complex, with their own table separate from a CreativeType and ResourceType Xref to specific Songs and Songbooks. But it reduces duplication of data across tables if you have lots of creators who appear multiple times across many roles.

## Technologies ##

* Asp.net Core 3.0
* Serilog
* StructureMap 
* Dapper
* Swagger
* SqlServer
* (Experimental) IdentityServer
* (Experimental) PolicyServer
* (Experimental) Docker
* (Super Experimental) Kubernetes

## Patterns and Practices ##

* API
    * GET/POST/PUT/DELETE/PATCH
    * RESTful API Implementation
    * HATEOAS
    * Pagination
    * Content and Media Type Definitions
    * (Unknown) Caching
* Data Access
    * Repository (Songbook as Aggregate Root)
    * Only allow queries on Indexed fields of database tables (Songbooks, Songs, Creators)
* General Development
    * Null-coalescing `ArgumentNullException` on constructor arguments (ex: `_logger = logger ?? throw new ArgumentNullException(nameof(logger))`)
    * Retrieval-levels on Repositories

## Technical Considerations ##

* Data access interfaces are defined in the model, but implemented in separate data access libraries specific to the data access technology being leveraged. (Dapper, Simple.Data, raw ADO, etc.)