
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

 1.In NetworkListner.Api pass valid 'IpAddress','Apiversion' as 1.0 and services is an optinal parameter.
 
 2.If you want specific service pass services as camma separate values like this GEOIP,RDAP,PING

# Docker Commands

Follow below mentioned commands to publish Api's into docker.It builds the containers of the individual docker files.

docker-compose up --build
