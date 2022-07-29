<walkthrough-metadata>
  <!-- <meta name="title" content="" /> -->
  <!-- <meta name="description" content="" /> -->
  <meta name="component_id" content="593554" />
  <meta name="unlisted" content="true" />
</walkthrough-metadata>

# Update inventory: add fulfillment places tutorial

## Introduction

Changes to a product's inventory information might occur more frequently than changes to its catalog information.

Instead of using the `UpdateRetailProduct` method to respecify an entire product when the fulfillment availabilities of a few specific places change, you can push incremental updates.

In such cases, `AddFulfillment` and `RemoveFulfillment` methods can be used to incrementally update product fulfillment. The place IDs are added to or removed from a given fulfillment type based on the fulfillment changes.

These methods are asynchronous because of downstream optimizations that support hundreds of concurrent updates per product without sacrificing performance.

For more information about managing catalog information, see the [Retail API documentation](https://cloud.google.com/retail/docs/inventory-updates#inventory-update-methods).

<walkthrough-tutorial-duration duration="5"></walkthrough-tutorial-duration>

## Get started with Google Cloud Retail

This step is required if this is the first Retail API tutorial you run.
Otherwise, you can skip it.
### Clone the Retail code samples

Clone the Git repository with all the code samples to learn the Retail features.

1. Run the following command in the Terminal:
    ```bash
    git clone https://github.com/GoogleCloudPlatform/dotnet-docs-samples.git
    ```

## Quick view on the Cloud Shell workspace

The Cloud Shell is a powerful IDE that allows you to create, build and run different projects. In this tutorial we will use some of its tools.

Since the repository is already cloned, the project files can be viewed in the <walkthrough-editor-spotlight spotlightid="file-explorer">`File Explorer`</walkthrough-editor-spotlight>

1. Click the Open folder button and chose the ```cloudshell_open``` folder.

1. Click any file and it will be opened in an editor.

You can modify the file and the changes will be applied.

To run the tutorial's commands you need to use the <walkthrough-editor-spotlight spotlightid="menu-terminal-new-terminal">`Terminal`</walkthrough-editor-spotlight>

## Prepare your work environment

To prepare the work environment you should perform the following steps:


### Select your project and enable the Retail API

Google Cloud organizes resources into projects. This lets you
collect all the related resources for a single application in one place.

If you don't have a Google Cloud project yet or you're not the owner of an existing one, you can
[create a new project](https://console.cloud.google.com/projectcreate).

After the project is created, set your PROJECT_ID to a ```project``` variable.
1. Run the following command in Terminal:
    ```bash
    gcloud config set project <YOUR_PROJECT_ID>
    ```

1. Check that the Retail API is enabled for your project in the [Admin Console](https://console.cloud.google.com/ai/retail/).

### Create service account

To access the Retail API you must create a service account.

1. To create service account follow this [instruction](https://cloud.google.com/retail/docs/setting-up#service-account)

1. Find your service account on the [IAM page](https://console.cloud.google.com/iam-admin/iam),
   click `Edit` icon and add the roles 'Storage Admin' and 'BigQuery Admin. It may take a while for the changes to take effect.

1. Copy the service account email in the field Principal.

### Set up authentication

To run a code sample from the Cloud Shell, you need to be authenticated using the service account credentials.

1. Login with your user credentials.
    ```bash
    gcloud auth login
    ```

1. Type `Y` and press **Enter**. Click the link in Terminal. A browser window should appear asking you to log in using your Gmail account.

1. Provide the Google Auth Library with access to your credentials and paste the code from the browser to the Terminal.

1. Upload your service account key JSON file and use it to activate the service account:

    ```bash
    gcloud iam service-accounts keys create ~/key.json --iam-account <YOUR_SERVICE_ACCOUNT_EMAIL>
    ```

    ```bash
    gcloud auth activate-service-account --key-file  ~/key.json
    ```

1. Set key as the GOOGLE_APPLICATION_CREDENTIALS environment variable to be used for requesting the Retail API:
    ```bash
    export GOOGLE_APPLICATION_CREDENTIALS= ~/key.json
    ```

**Note**: Click the copy button on the side of the code box to paste the command in the Cloud Shell terminal and run it.


### Set the PROJECT_NUMBER and PROJECT_ID environment variables

Because you are going to run the code samples in your own Google Cloud project, you should specify the **project_number** and **project_id** as environment variables. It will be used in every request to the Retail API.

1. Find the project number and project ID in the Project Info card displayed on **Home/Dashboard**.

1. Set **project_number** with the following command:
    ```bash
    export PROJECT_NUMBER=<YOUR_PROJECT_NUMBER>
    ```
1. Set **project_id** with the following command:
    ```bash
    export PROJECT_ID=<YOUR_PROJECT_ID>
    ```

### Install Google Cloud Retail libraries

To run .NET code samples for the Retail API tutorial, you need to set up your virtual environment.

1. Next, install Google packages:
    ```bash
    for service_dir in \
        {RetailEvents.Samples,RetailProducts.Samples,RetailSearch.Samples}
    do
        path=~/cloudshell_open/dotnet-docs-samples/retail/interactive-tutorial/$service_dir
    cd $path
    dotnet add package \
        Google.Cloud.Retail.V2
    dotnet add package \
        Google.Cloud.Storage.V1
    done

    ```

## Clone the Retail code samples

This step is required if this is the first Retail API tutorial you run.
Otherwise, you can skip it.

Clone the Git repository with all the code samples to learn the Retail features and check them in action.

1. Run the following command in the Terminal:
    ```bash
    git clone https://github.com/GoogleCloudPlatform/dotnet-docs-samples.git
    ```

    The code samples for each of the Retail services are stored in different directories.

1. Go to the ```.../RetailProducts.Samples``` directory. This is our starting point to run more commands.
    ```bash
    cd dotnet-docs-sample/retail/interactive-tutorial/RetailProducts.Samples
    ```

## Add fulfillment places

1. Set the following fields to send the `AddFulfillmentPlacesRequest` request:
    - `Product` — the product name whose inventory information will be updated
    - `Type` — the fulfillment type. You can set one of the [supported values](https://cloud.google.com/retail/docs/reference/rpc/google.cloud.retail.v2#addfulfillmentplacesrequest).
    - `placeIds[]` — the store IDs for each of the fulfillment types
    - `AddTime` — the time when the fulfillment updates are pushed. It is used to prevent out-of-order updates on the fulfillment information. If this isn't provided, the internal system time will be used.
    - `AllowMissing` — if set to true and the product is not found, the fulfillment information will be retained for up to 24 hours and processed after the product is created.

1. Open <walkthrough-editor-select-regex filePath="cloudshell_open/dotnet-docs-samples/retail/interactive-tutorial/RetailProducts.Samples/AddFulfillmentPlaces.cs" regex="private static AddFulfillmentPlacesRequest GetAddFulfillmentRequest">.../RetailProducts.Samples/AddFulfillmentPlaces.cs</walkthrough-editor-select-regex> file and check the `AddFulfillmentPlacesRequest` request.

1. To add the fulfillment places, open the Terminal and run the following command:
    ```bash
    dotnet run -- AddFulfillmentPlacesTutorial
    ```

1. Check the response in the Terminal. The product is initially created without the fulfillment information. Check the `GetRetailProduct()` response with the fulfillment places `store2`, `store3` and `store4` added to the `pickup-in-store` fulfillment type.

## Send an out-of-order AddFulfillmentPlaces request

The `AddFulfillment` method lets you specify the update time when the request is sent.
The Retail API compares the update time you've specified with the latest time recorded for the relevant inventory fields. The update happens only if the specified update time value is greater than the latest update time value.

1. Modify the <walkthrough-editor-select-regex filePath="cloudshell_open/dotnet-docs-samples/retail/interactive-tutorial/RetailProducts.Samples/AddFulfillmentPlaces.cs" regex="string[] placeIds">`placeIds`</walkthrough-editor-select-regex> field value:
    ```
    string[] placeIds = { "store2", "store3", "store4" };
    ```

1. Uncomment line and set the <walkthrough-editor-select-regex filePath="cloudshell_open/dotnet-docs-samples/retail/interactive-tutorial/RetailProducts.Samples/AddFulfillmentPlaces.cs" regex="// requestTimeStamp">`requestTimeStamp`</walkthrough-editor-select-regex> value to yesterday:
    ```
    requestTimeStamp = DateTime.Now.ToUniversalTime().AddDays(-1);
    ```

1. Run the code sample in the Terminal:
    ```bash
    dotnet run -- AddFulfillmentPlacesTutorial
    ```

1. Check the product printed out in the Terminal. The fulfillment places information has no updates.

## Congratulations

<walkthrough-conclusion-trophy></walkthrough-conclusion-trophy>

You have completed the tutorial! We encourage you to test adding the product fulfillment places by yourself.

<walkthrough-inline-feedback></walkthrough-inline-feedback>

### Do more with the Retail API

<walkthrough-tutorial-card id="retail_api_v2_set_invenory_dotnet" icon="LOGO_DOTNET" title="Set inventory tutorial" keepPrevious=true>
Try to get a product via the Retail API</walkthrough-tutorial-card>

<walkthrough-tutorial-card id="retail_api_v2_remove_fulfillment_places_dotnet" icon="LOGO_DOTNET" title="Removee fulfillment tutorial" keepPrevious=true>Try to update a product via the Retail API</walkthrough-tutorial-card>
