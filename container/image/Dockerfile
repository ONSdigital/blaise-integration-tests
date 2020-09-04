# identical to blaise-test-base in blaise-setup-gcp, just different entrypoint
FROM python:3-slim

ADD requirements.txt /tmp/requirements.txt

run apt-get update ; \
    apt-get install -qq -y jq git ; \
    pip install -q --no-cache-dir -r /tmp/requirements.txt ; \
    apt-get purge -y git ; \
    rm -rf /var/lib/apt/lists/*

CMD ["cd /data ; behave"]
