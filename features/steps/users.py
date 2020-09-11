import os
import logging
import requests

from pyblaise import Blaise
from bs4 import BeautifulSoup

from behave import given, when, then, step

logger = logging.getLogger(__name__)
logger.setLevel(os.getenv("LOG_LEVEL", logging.WARN))


@given("I am logged in to {system}")
def step_impl(context, system):
  context.system = system

  if system == "blaise":
    context.username = os.environ["BLAISE_USERNAME"]
    context.password = os.environ["BLAISE_PASSWORD"]
    logger.info("connecting to blaise on '%s:%s' as '%s'" % (
        os.environ["BLAISE_HOSTNAME"], os.environ["BLAISE_HOSTPORT"], os.environ["BLAISE_USERNAME"]))
    context.token = Blaise("https",
                           os.environ["BLAISE_HOSTNAME"],
                           int(os.environ["BLAISE_HOSTPORT"]),
                           context.username, context.password).token
  elif system == "bsm":
    context.username = os.environ["BSM_USERNAME"]
    context.password = os.environ["BSM_PASSWORD"]
    logger.info("connecting to BSM on '%s' as '%s'" % (
        os.environ["BSM_HOSTNAME"], os.environ["BSM_USERNAME"]))

    R = requests.get("https://%s/login" % os.environ["BSM_HOSTNAME"])

    R = requests.post("https://%s/login" % os.environ["BSM_HOSTNAME"],
                      data={"username": os.environ["BSM_USERNAME"],
                            "password": os.environ["BSM_PASSWORD"]}
                     )
    logger.debug("POST /login [%03i]" % R.status_code)
    logger.debug("%s" % R.text)

    assert "You need to login to access this page" not in R.text, "ERR: could not login to blaise as '%s'" % context.username
    context.token = R.headers["Set-Cookie"]
    logger.debug("cookie: '%s'" % context.token)


  assert context.token is not None, "No token received by login"


@when("I add a {role} user")
def step_impl(context, role):
  if context.system == "bsm":
    # do a get request to get the csrf token
    R = requests.get("https://%s/users/add_user" % os.environ["BSM_HOSTNAME"],
                     headers={"cookie": context.token})
    logger.debug(R)
    html = BeautifulSoup(R.text, "html.parser")
    assert len(html) > 0, "HTML response is empty"
    csrf_token = html.find(id="csrf_token")["value"]

    # post our new user data
    user_data = {
        "username": "gherkin-user",
        "password": "gh3rk1n-u53r",
        "firstname": "gherkin-user",
        "surname": "gherkin-user",
        "role": role,
        "csrf_token": csrf_token
    }
    R = requests.post("https://%s/users/add_user" % os.environ["BSM_HOSTNAME"],
                      data=user_data,
                      headers={"Content-Type": "application/x-www-form-urlencoded",
                               "cookie": context.token})
    logger.debug(R)
  else:
    raise NotImplementedException

@then("the {role} user exists")
def step_impl(context, role):
  if context.system == "bsm":
    R = requests.get("https://%s/users/list_users" % os.environ["BSM_HOSTNAME"],
                     headers={"cookie": context.token})
    logger.debug(R)
    html = BeautifulSoup(R.text, "html.parser")
    assert len(html) > 0, "HTML response is empty"
  else:
    raise NotImplementedException
