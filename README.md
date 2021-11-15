
# NetworkListner.API

The application built with microservices architecture.NetworkListner.API is the main API which process list of services parllelly.
It makes the requests to services(Geo.Api,Rdns.Api,Rdap.Api and Ping.Api) and seggrigate the all the response. 

# Swagger Url

NetworkListner.Api --  http://localhost:5000/swagger/index.html

GeoIp.Api--http://localhost:5001/swagger/index.html

Rdap.Api-- http://localhost:5002/swagger/index.html

Rdns.Api--http://localhost:5003/swagger/index.html

Ping.Api--http://localhost:5004/swagger/index.html

# Test Steps 

 In NetworkListner.Api pass the below mentioned input parametes in swagger. 
 
       IpAddress --Any valid Ip address
       Services -- GEOIP,RDAP,PING
       Version -- 1.0
       
  Note:Service is an optional parameter.If we not pass any value it get the services from configuration and process.      

# Docker Commands

Follow below mentioned commands to publish Api's into docker.It builds the containers of the individual docker files.

Go to the root folder of the project and run below mentioned command through command line.

docker-compose up --build
