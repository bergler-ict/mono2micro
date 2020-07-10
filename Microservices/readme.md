       _____       __          _____                 _         
      / ___/____ _/ /__  _____/ ___/___  ______   __(_)_______ 
      \__ \/ __ `/ / _ \/ ___/\__ \/ _ \/ ___/ | / / / ___/ _ \
     ___/ / /_/ / /  __(__  )___/ /  __/ /   | |/ / / /__/  __/
    /____/\__,_/_/\___/____//____/\___/_/    |___/_/\___/\___/ 
                                                           
---------------------------------------------
## prerequisites 
- dotnet core 3.1 (https://dotnet.microsoft.com/download/dotnet-core/3.1)
- SQL Server local: (https://www.microsoft.com/nl-nl/sql-server/sql-server-downloads) or azure instance.
- ef core tools (https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet)
- docker for windows (https://docs.docker.com/docker-for-windows/install/

running instance of RabbitMQ
<pre>
<code>
docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
</code>
</pre>
Or use the cloud variant (https://www.cloudamqp.com/) There is a free tier which is more than sufficient for this demo.

## Projects
This repo contains multiple projects as a sample to illustrate the working of the sales microservice. The following projects are part of this repo.
### SalesService.Web
Set the connectionstring to you database in the appsettings.
This project also contains the consumer for the product changed event which are published by the ProductService.Stub.Web. *This project connects to your running instance of RabbitMQ, so before you run this project please start RabbitMQ first. (see prerequisites)*
The connection to RabbitMQ is also set in the appsettings.json

This project contains the actual API's, for the shoppingbasket. Besides the API's does this project contain the following endpoints:
- https://localhost:5001/swagger/index.html
- https://localhost:5001/health

### SalesService.Data
This project contains the datamodels,dbcontext and migrations for EntityFramework core. 
#### database setup
To Setup the database go to the SalesService.Data folder and run.
<pre><code>dotnet ef migrations add InitialCreate --startup-project ../SalesService.Web
dotnet ef database update --startup-project ../SalesService.Web
</code></pre>
*Note:If you want you change the connectionstring to the db please don't forget to also change it in the SalesDesignTimeDbContextFactory.cs*

### SalesService.Web.Test
This project contains the component integration tests for the SalesService.Web project bases on the WebApplicationFactory. All test are run against an in memory database.

### SalesService.Data.Test
This project contains the unit tests for the SalesService.Data project. All test are run against an in memory database by default. If you want to seed your database with some valid products you can uncomment the UseSqlServer line in SalesServiceDatabase.cs file. (and comment out the UseInMemoryDatabase)
<pre><code>
.UseInMemoryDatabase("InMemoryDbForTesting")
//.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"))
</code></pre>

        ____                 __           __ _____                 _            _____ __        __  
       / __ \_________  ____/ __  _______/ // ___/___  ______   __(_________   / ___// /___  __/ /_ 
      / /_/ / ___/ __ \/ __  / / / / ___/ __\__ \/ _ \/ ___| | / / / ___/ _ \  \__ \/ __/ / / / __ \
     / ____/ /  / /_/ / /_/ / /_/ / /__/ /____/ /  __/ /   | |/ / / /__/  ___ ___/ / /_/ /_/ / /_/ /
    /_/   /_/   \____/\__,_/\__,_/\___/\__/____/\___/_/    |___/_/\___/\___(_/____/\__/\__,_/_.___/ 
                                                                                                
                                                        
Solely for testing purposes we added a ProductService.Stub. This is to illustrate how you can populate the replicated data in the SalesService microservice. 
### ProductService.Stub.Web
This project would normaly contain the implementation of the productservice but now only publishes an event if a product is updated or added. This project contains the following endpoint:
- https://localhost:5003/swagger/index.html

you can use the swagger ui to publish an event. *This project connects to your running instance of RabbitMQ, so before you run this project please start RabbitMQ first. (see prerequisites)*
The connection to RabbitMQ is also set in the appsettings.json

### ProductService.Stub.MessageContracts
The structure of the message which is published by the ProductService.Stub.Web is stored in this shared library. So that both the ProductService.Stub.Web and the SalesService.Web know the structure.

# Hosting
If you want to host these services in the cloud using containers you could follow the following sample.
This sample is based on Azure and Kubernetes.
You can build the services using the following dockerfiles.
<pre><code>
docker build . -f .\ps.Dockerfile --tag productservice:1.0
docker build . -f .\ss.Dockerfile --tag salesservice:1.0
docker tag productservice:1.0 [registry_name]/demo/productservice:1.0
docker tag salesservice:1.0 [registry_name]/demo/salesservice:1.0
docker push [registry_name]/demo/productservice:1.0
docker push [registry_name]/demo/salesservice:1.0
</code></pre>

To deploy these service to Kubernetes you could use the deployment.yaml
First you need to change the deployment.yaml so that the images are pulled from the correct registry.
<pre><code>
kubectl apply -f ./deployment.yaml
</code></pre>




