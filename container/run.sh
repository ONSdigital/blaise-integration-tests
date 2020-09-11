#!/bin/bash -u

docker run \
  -it \
  --rm \
  --name bha \
  -v $(pwd):/data \
  -e BLAISE_USERNAME=blaise \
  -e BLAISE_PASSWORD=dummy_pass3 \
  -e BLAISE_HOSTNAME=blaise_hostname \
  -e BLAISE_PORT=8031 \
  -e BSM_USERNAME=admin \
  -e BSM_PASSWORD=admin \
  -e BSM_HOSTNAME=bsm_hostname \
  -e LOG_LEVEL=DEBUG \
  behave \
  /bin/bash -c "cd /data; behave -n 'add a survey'"

#  /bin/bash -c "cd /data; behave --no-color --no-capture --no-logcapture"
#  /bin/bash -c "cd /data; behave"
