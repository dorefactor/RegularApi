# Regular API

Configuration API for Regular Deployer, it allows users to configure the applications that are going to be deployed by the tool. Also, this API provides the entrypoint where a deployment can be triggered.

## Service Dependencies

Regular API depends on:

* RabbitMQ, used as a service bus.
* MongoDB, used as repository for application configurations.

## Running the API

In order to execute this API it is necessary to set the following settings:

* RabbitMq:Server, Rabbit MQ host
* MongoDb:Server, Mongo DB host
