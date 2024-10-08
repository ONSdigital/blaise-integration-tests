# Blaise Integration Tests

This repository contains a subset of our integration tests for Blaise and some related wrapper services. Please note that this does not include all of our integration tests, as others are located in repositories alongside their corresponding services.

Some of these tests use Selenium to interact with various UIs and are written in C# to leverage the Blaise NuGet package for communication with Blaise.

Both building and running the integration tests are managed through Concourse jobs. These jobs are automatically triggered on pushes to this repository as well as to the corresponding repositories that these integration tests cover. The Concourse jobs securely call Azure DevOps pipelines via HTTP requests, where the tests are executed using hosted Azure DevOps agents.

## Local setup

Since Blaise currently only provides a .NET Framework NuGet API, these tests require a Windows environment.

It's recommended to run the tests locally while connecting to your GCP sandbox environment.

### Install tools

You'll need the following tools installed:

- Visual Studio
- gcloud CLI
- Chrome
- ChromeDriver

You may want to consider using the Windows package manager [Chocolatey](https://chocolatey.org/) to install these tools.

### Setup Blaise license

To run the tests, you must have a valid license for the local version of Blaise. The tests utilise the Blaise NuGet API, which checks the license status.

License information can be found in the metadata of the `blaise-gusty-mgmt` VM.

To register the license, you have two options:

- Install Blaise and follow the registration steps
- Manually add the registration details to the Windows registry

If you choose to add the details to the Windows registry, create a `.reg` file with the following content:

```
Windows Registry Editor Version 5.00

[HKEY_LOCAL_MACHINE\SOFTWARE\StatNeth\Blaise\5.0]
"Licensee"=""
"LicenseKey"=""
"ActivationCode"=""
```

### Download test questionnaire

Most of the tests require a `.bpkg` file, which is a Blaise package containing a questionnaire instrument. The tests prepare the testing environment by deploying this test questionnaire.

You can download the latest test questionnaire from [Confluence](https://confluence.ons.gov.uk/display/QSS/Blaise+5+Questionnaire+Instrument+Artefacts) or the [Blaise shared GCP storage bucket](https://console.cloud.google.com/storage/browser?project=ons-blaise-v2-shared).

### Configure the solution

[Add our Azure DevOps artifacts feed to Visual Studio.](https://confluence.ons.gov.uk/display/QSS/How-to+connect+to+our+private+NuGet+package+source)

Git clone down this repository and open the solution file `BlaiseIntegrationTests.sln` in Visual Studio.

Depending on the tests your running, substitute the environment variable values in the `app.config` files with the following:

```xml
  <appSettings>
	  <add key="UninstallSurveyTimeOutInSeconds" value="5" />
	  <add key="QuestionnairePath" value="C:\<test-questionnaire-path>\" />
	  <add key="ServerParkName" value="gusty" />
	  <add key="QuestionnaireName" value="DST2304Z" />
	  <add key="ENV_BLAISE_SERVER_HOST_NAME" value="localhost" />
	  <add key="ENV_BLAISE_ADMIN_USER" value="<blaise-username>" />
	  <add key="ENV_BLAISE_ADMIN_PASSWORD" value="<blaise-password>" />
	  <add key="ENV_BLAISE_SERVER_BINDING" value="http" />
	  <add key="ENV_BLAISE_CONNECTION_PORT" value="8031" />
	  <add key="ENV_BLAISE_REMOTE_CONNECTION_PORT" value="8033" />
	  <add key="ENV_CONNECTION_EXPIRES_IN_MINUTES" value="60" />
	  <add key="ChromeWebDriver" value="C:\<chrome-driver-path>\" />
	  <add key="ENV_DQS_URL" value="https://dev-<sandbox>-dqs.social-surveys.gcp.onsdigital.uk" />
	  <add key="ENV_TOBI_URL" value="https://dev-<sandbox>-tobi.social-surveys.gcp.onsdigital.uk" />
	  <add key="ENV_BLAISE_CATI_URL" value="https://dev-<sandbox>-cati.social-surveys.gcp.onsdigital.uk" />
  </appSettings>
```

Placeholder | Description
--- | ---
`<test-questionnaire-path>` | Local path to the test questionnaire package file.
`<blaise-username>`         | From the `blaise-gusty-mgmt` VM `ENV_BLAISE_ADMIN_USER` environment variable.
`<blaise-password>`         | From the `blaise-gusty-mgmt` VM `ENV_BLAISE_ADMIN_PASSWORD` environment variable.
`<sandbox>`                 | The short name of your sandbox environment. e.g. `rr5`.
`<chrome-driver-path>`      | Local path to the Chrome driver executable file.

:rotating_light: **IMPORTANT: DO NOT COMMIT APP.CONFIG FILES WITH ACTUAL VALUES!** :rotating_light:

Build the solution via the Visual Studio `Build` menu.

### Create tunnels to Blaise management VM

If you have Blaise installed locally, you'll need to stop the `BlaiseServices5` service from running.

Authenticate the gcloud CLI:

```
gcloud auth login
```

Set gcloud to your sandbox environment project:

```
gcloud config set project ons-blaise-v2-dev-<sandbox>
```

Open a tunnel for port 8031 to the Blaise management VM in your sandbox environment:

```
gcloud compute start-iap-tunnel blaise-gusty-mgmt 8031 --local-host-port=localhost:8031
```

In a separate instance, open a tunnel for port 8033 to the Blaise management VM in your sandbox environment:

```
gcloud compute start-iap-tunnel blaise-gusty-mgmt 8033 --local-host-port=localhost:8033
```

### Run the tests

Open `Test Explorer` from the Visual Studio `View` menu.

Run all the tests or specific tests using the play buttons.

## Troubleshooting

If the `Test Explorer` window doesn't show any tests, clean and rebuild the solution via the Visual Studio `Build` menu.

When using Chocolatey to install packages, ensure you are running your Command prompt or PowerShell instance as administrator.

Trying to find the path to chromedriver.exe? Chocolatey should put it in `C:\tools\selenium\` by default.

If you get a `.ps1 is not digitally signed` message when trying to use packages installed by Chocolatey in PowerShell, run the following:

```
Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass
```

This will need to be run for every PowerShell instance.
