# Current Status (Badge)
[![Build Status](https://dev.azure.com/blaise-gcp/csharp/_apis/build/status/ONSdigital.blaise-automated-tests?branchName=refs%2Fpull%2F7%2Fmerge)](https://dev.azure.com/blaise-gcp/csharp/_build/latest?definitionId=43&branchName=refs%2Fpull%2F7%2Fmerge)

# blaise-automated-tests
Repository of automated testing processes for the Blaise 5 system.

## Running from a local development machine

To develop these tests, the quickest way is to run them locally with them
running against your sandbox. To do this you will need to be using a Windows
development machine.

### Install Tools

You need the following tools installed. **These must be installed on Windows,
NOT WSL**.

- Chrome
- The gcloud CLI.
- chromedriver
- Visual Studio 2022

An easy way to install tools on Windows is by using
[Chocolatey](https://chocolatey.org/).

### Log in to GCP

In PowerShell, run:

```powershell
gcloud auth login
gcloud config set project <your-project-name>
```

### Set up the Blaise License

You will need to add a Blaise License key to your development machine. To do
this, you will need to get the values from the `blaise-gusty-mgmt` VM in your
sandbox and add them to your registry.

To get the values from the VM, you can run the following command and look for
the `key`/`value` pairs under `metadata.items`.

```powershell
add le commande here
```

To edit the registry, you need to open the **Registry Editor**. You can do this
quickly by tapping the **Windows Key** and then typing `reg` - an icon for the
Registry Editor should appear. 

The value will need to go the the following location in the registry to add the
values. If the location doesn't exist, you'll need to create it by
right-clicking on relevant _folders_ and choosing **New -> Key**.

Location: `HKEY_LOCAL_MACHINE\SOFTWARE\StatNeth\Blaise\5.0`

The values to need to move across are:

| VM Environment Variable | Registry (String Value) |
|-------------------------|-------------------------|
| `BLAISE_LICENSEE`       | `Licensee`              |
| `BLAISE_ACTIVATIONCODE` | `ActivationCode`        |
| `BLAISE_SERIALNUMBER`   | `LicenseKey`            |

### Download the Questionnaire

Assuming that your sandbox has fully deployed, you should be able to download
the test questionnaire from the `ons-blaise-v2-dev-to1-dqs` bucket.

The quickest way to do this is use the `gsutil` command:

```powershell
gsutil cp gs://ons-blaise-v2-dev-to1-dqs/DST2111Z.bpkg C:\<path-to-store-questionnaire>\
```

### Configure the Project

**IMPORTANT: Ensure you have [added the NuGet source on your computer](https://confluence.ons.gov.uk/display/QSS/How-to+update+the+StatNeth+Blaise+API+NuGet+package)**

Clone this repository on to your laptop and open
[BlaiseAutomatedTests.sln](./BlaiseAutomatedTests.sln) in Visual Studio.

You will need to update the `app.config` files with the following values:

```xml
  <appSettings>
	  <add key="InstrumentPath" value=" C:\<path-to-store-questionnaire>"/>
	  <add key="ServerParkName" value="gusty"/>
	  <add key="InstrumentName" value="DST2111Z"/>
	  <add key="ENV_BLAISE_SERVER_HOST_NAME" value="localhost"/>
	  <add key="ENV_BLAISE_ADMIN_USER" value="<blaise-username>"/>
	  <add key="ENV_BLAISE_ADMIN_PASSWORD" value="<blaise-password>"/>
	  <add key="ENV_BLAISE_SERVER_BINDING" value="http"/>
	  <add key="ENV_BLAISE_CONNECTION_PORT" value="8031"/>
	  <add key="ENV_BLAISE_REMOTE_CONNECTION_PORT" value="8033"/>
	  <add key="ENV_CONNECTION_EXPIRES_IN_MINUTES" value="90"/>
	  <add key="ENV_TOBI_URL" value="dev-<sandbox>-tobi.social-surveys.gcp.onsdigital.uk"/>
	  <add key="ENV_BLAISE_CATI_URL" value="dev-<sandbox>-cati.social-surveys.gcp.onsdigital.uk"/>
      <add key="ChromeWebDriver" value="C:\<path-to-chromedriver" />
  </appSettings>
```

The values you need to substitute in are:

| `<path-to-store-questionnaire>` | When ever path you used to download the questionnaire to. **This does not include the filename or a trailling slash**. |
| `<blaise-username>`             | From the `blaise-gusty-mgmt` VM `ENV_BLAISE_ADMIN_USER` environment variable.                                          |
| `<blaise-password>`             | From the `blaise-gusty-mgmt` VM `ENV_BLAISE_ADMIN_PASSWORD` environment variable.                                      |
| `<sandbox>`                     | The short name of your sandbox. e.g. `rr5`.                                                                            |
| `<path-to-chromedriver>`        | The path to where the `chromedriver.exe` exists. **This does not include `chromedriver.exe` or the trailing slash**.   |

### Create the tunnels

You need to create two (I think) tunnels to the Blaise Management VM.

Open a new PowerShell window and run:

```powershell
gcloud compute start-iap-tunnel blaise-gusty-mgmt 8031 --local-host-port=localhost:8031
```

Then open second PowerShell window and run:

```powershell
gcloud compute start-iap-tunnel blaise-gusty-mgmt 8033 --local-host-port=localhost:8033
```
### Run the tests

You should now be able to build and run the tests in Visual Studio.

### Things for Mac developers to remember

* When using Chocolatey to install packages, ensure you are running Powershell as Adminstrator:
	- Search for Powershell in the Start menu
	- Right click, and select 'Run as administrator'
* After installing new packages, Visual Studio will need to be restarted before it will recognise new packages
* Remember to save your files before trying to commit to Git. Pycharm saves automatically. JustSaying!

### Troubleshooting

## Chocolatey

After installing packages using Chocolatey, if you receive the following error when trying to execute package commands:

```powershell
.ps1 is not digitally signed. You cannot run this script on the current system.
```

You will need to execute the following, per https://caiomsouza.medium.com/fix-for-powershell-script-not-digitally-signed-69f0ed518715:

```powershell
Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass
```