import os
import logging
import requests

from pyblaise import Blaise
from bs4 import BeautifulSoup

from behave import given, when, then, step

logger = logging.getLogger(__name__)
logger.setLevel(os.getenv("LOG_LEVEL", logging.WARN))


@when("I add a survey")
def step_impl(context):
  if context.system == "bsm":
    # create a survey name
    context.survey_name = "my-survey-name"

    logger.debug("cookie: '%s'" % context.token)

    # do a get request to get the csrf token
    R = requests.get("https://%s/survey_config/add_survey" % os.environ["BSM_HOSTNAME"],
                     headers={"cookie": context.token})
    logger.debug("GET /survey_config/add_survey [%03i]" % (R.status_code))


    html = BeautifulSoup(R.text, "html.parser")
    assert len(html) > 0, "HTML response is empty"
    csrf_token = html.find(id="csrf_token")["value"]

    # post our new survey data
    # FIXME: create a matrix of survey types
    survey_data = {
        "tla": "TST",
        "survey_name": context.survey_name,
        "csrf_token": csrf_token
    }
    R = requests.post("https://%s/survey_config/add_survey" % os.environ["BSM_HOSTNAME"],
                      data=survey_data,
                      headers={"Content-Type": "application/x-www-form-urlencoded",
                               "cookie": context.token})
    logger.debug("POST /survey_config/add_survey [%03i]" % R.status_code)
  else:
    raise NotImplementedException


@then("the survey does exist")
def step_impl(context):
  if context.system == "bsm":
    R = requests.get("https://%s/survey_config/list_surveys" % os.environ["BSM_HOSTNAME"],
                     headers={"cookie": context.token})
    logger.debug("GET : '%s'" % str(R))

    html = BeautifulSoup(R.text, "html.parser")
    assert len(html) > 0, "HTML response is empty"

    assert context.survey_name in R.text, "ERR: '%s' not found in reponse" % context.survey_name
  else:
    raise NotImplementedException
