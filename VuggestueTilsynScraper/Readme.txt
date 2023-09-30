RUN Scraper in docker


For the Scraper can publish to rabbit it have to establish a connection. For lthis to happen it have to run on the same Docker networt.

See which docker network trabbit is running on: 
- docker network ls : See networks
- docker inspect <network>


For running the the scraper on the docker network
- Build the image in Visual Studio by right click the image
- Run the image on the same network as the rabbit: docker run --network=<correct network> <image name>