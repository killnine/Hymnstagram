# Hymnstagram #

[![Build status](https://ci.appveyor.com/api/projects/status/b83gpw3lahy8g516?svg=true)](https://ci.appveyor.com/project/killnine/hymnstagram)


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

* Asp.net Core 3.0 - Done
* Serilog          - Done
* StructureMap  
* Dapper
* Swagger          - Done
* FluentValidation - Done
* Marvin.Cache.Headers - Done
* (Miniprofiler)[https://miniprofiler.com/dotnet/ConsoleDotNet]
* SqlServer
* (Experimental) IdentityServer
* (Experimental) PolicyServer
* (Experimental) Docker
* (Experimental) (Benchmark.net)[https://benchmarkdotnet.org]
* (Super Experimental) Kubernetes

## Patterns and Practices ##

* API
    * GET/POST/PUT/DELETE/PATCH    
    * HATEOAS
    * Pagination
      * X-Pagination header    
    * Validation (Domain)
      * FluentValidation      
    * Caching & Concurrency
      * ETags
      * Cache Headers
    * Documentation
      * Swagger/Swashbuckle
      * XML Comments and #pragma
      * 'ProducesResponseType' for status codes
      * 'Produces' for content type
      * 'Consumes' for content type
      * ActionResult<T>
      
* Data Access
    * Repository (Songbook as Aggregate Root)
    * Only allow queries on Indexed fields of database tables (Songbooks, Songs, Creators)
    * Model project defines interface of DAOs. Separate project for implementation

* General Development
    * Options pattern for application settings (Pluralsight)[https://app.pluralsight.com/library/courses/dotnet-core-aspnet-core-configuration-options/table-of-contents]
    * Null-coalescing `ArgumentNullException` on constructor arguments (ex: `_logger = logger ?? throw new ArgumentNullException(nameof(logger))`)
    * Retrieval-levels on Repositories
