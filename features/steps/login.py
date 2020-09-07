import os
import requests

from pyblaise import Blaise

from behave import given, when, then, step


@given("a valid username for {system}")
def step_impl(context, system):
  context.system = system

  if system == "blaise":
    context.username = os.environ["BLAISE_USERNAME"]
    context.password = os.environ["BLAISE_PASSWORD"]
  elif system == "bsm":
    context.username = os.environ["BSM_USERNAME"]
    context.password = os.environ["BSM_PASSWORD"]


@given("an invalid username for {system}")
def step_impl(context, system):
  context.system = system
  context.username = "baddfood"
  context.password = "baddbeef"


@when("I log in")
def step_impl(context):
  if context.system == "blaise":
    hostname = os.environ["BLAISE_HOSTNAME"]
    port = int(os.environ["BLAISE_PORT"])

    b = Blaise("https", hostname, port, context.username, context.password)

    context.token = b.token
  elif context.system == "bsm":
    hostname = os.environ["BSM_HOSTNAME"]

    R = requests.post("https://%s/login" % hostname,
                      data={"username": context.username,
                            "password": context.password}
                     )

    print("status: %i" % R.status_code)
    print("headers: '%s'" % R.headers)
    assert R.status_code >= 200 and R.status_code <= 302
    context.token = R.headers["Set-Cookie"]
    context.response = R.text


@then("I am logged in")
def step_impl(context):
  if context.system == "blaise":
    # FIXME: check the token matches a regex
    assert context.token is not None
  elif context.system == "bsm":
    assert "Sign out" not in context.response, "ERR: 'Sign out' text not found in response"


@then("I am not logged in")
def step_impl(context):
  if context.system == "blaise":
    assert context.token is None
  elif context.system == "bsm":
    assert "Invalid" not in context.response, "ERR: 'Invalid' text not found in response"
