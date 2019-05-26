# **Regular API**

Configuration API for Regular Deployer, it allows users to configure the applications that are going to be deployed by the tool. Also, this API provides the entrypoint where a deployment can be triggered.

## **Prerequisites**

* NetCore 2.2+
* RabbitMQ 3+
* MongoDB 4+
* [Jenkins Builder](https://github.com/dorefactor/rd-jenkins-deployer) for deployments

## **Service Dependencies**

Regular API depends on:

* RabbitMQ, used as a service bus.
* MongoDB, used as repository for application configurations.

## **Configure the API**

In order to execute this API it is necessary to set the following settings:

* RabbitMq:Server, Rabbit MQ host
* MongoDb:Server, Mongo DB host
* Jenkins:Url, Jenkins host

RegularAPI will look for user and credentials in the following environment variables:

* `RABBIT_USER`: Rabbit MQ user
* `RABBIT_PASSWORD`: Rabbit MQ password
* `MONGO_USER`: MongoDB user (with privileges to write into configured database)
* `MONGO_PASSWORD`: MongoDB password
* `JENKINS_USER`: Jenkins user
* `JENKINS_PASSWORD`: Jenkins password

### **Custom Configuration**

#### **Rabbit MQ**

By default the API will use the Rabbit MQ exchange `regular-deployer-exchange`, listen the queue: `com.dorefactor.deploy.command` and host: `rabbitmq-host`, those values can be changed by editing the file `appsettings.json` using the keys:

* `RabbitMq:Exchange`
* `RabbitMq:CommandQueue`
* `RabbitMq:Server`

#### **MongoDB**

By default the host `mongodb-host` will be used and database: `regularOrchestrator`, those values can be overrided by editing `appsettings.json` using the keys:

* `MongoDb:Server`
* `MongoDb:Database`

## **Build**

This application was created using [.NET Core](https://dotnet.microsoft.com/) 2.2, in order to build the application the following commands need to be executed from shell:

1. `dotnet restore`
2. `dotnet build`

## **Unit Tests**

Unit test were created using NUnit and can be executed with the command `dotnet test`. If more detail in test are desired command `dotnet test -v n` can be executed.

## **Execute the application**

In order to execute the API just run the command `dotnet run`.
