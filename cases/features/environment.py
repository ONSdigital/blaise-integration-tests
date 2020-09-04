import os
import sys
import logging

from behave import fixture

@fixture
def create_user(context, username, password):
    context.user = {"username": username, "password": password}
    yield context.user


logging.basicConfig(level=os.getenv("LOG_LEVEL", logging.DEBUG), stream=sys.stdout)

def before_feature(context, feature):
    logging.info("test logging")
