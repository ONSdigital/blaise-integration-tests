#!/bin/bash -u

docker run \
  -it \
  --rm \
  --name bha \
  -v $(pwd):/data \
  -e BLAISE_USERNAME=blaise \
  -e BLAISE_PASSWORD=bKaNalC6FSxpVGsW \
  -e BLAISE_HOSTNAME=dev-ed-84-client-tel.social-surveys.gcp.onsdigital.uk \
  -e BLAISE_PORT=8031 \
  -e BSM_USERNAME=admin \
  -e BSM_PASSWORD=admin \
  -e BSM_HOSTNAME=dev-ed-84-bsm.social-surveys.gcp.onsdigital.uk \
  -e LOG_LEVEL=DEBUG \
  behave \
  /bin/bash -c "cd /data; behave -n 'add a survey'"

#  /bin/bash -c "cd /data; behave --no-color --no-capture --no-logcapture"
#  /bin/bash -c "cd /data; behave"
