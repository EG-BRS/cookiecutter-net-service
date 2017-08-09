def projectName = "demoProject";
def pathName = "EG.One.DotNetCoreTemplate";
def majorVersion = 1;
def minorVersion = 0;
def buildVersion = env.BUILD_NUMBER;
def version = "$majorVersion.$minorVersion.$buildVersion";

node { 
    stage('Build') {
        checkout scm

        sh "dotnet restore --configfile=Nuget.Config"
        sh "dotnet publish -c Release"
    }
    stage('Unit tests') {
        sh "dotnet restore test/${projectName}.UnitTest"
        sh "dotnet test test/${projectName}.UnitTest/${projectName}.UnitTest.csproj"
    }
    stage('Package') {
        if (env.BRANCH_NAME  == "develop") {
            docker.withRegistry("https://kalk1-on.azurecr.io", "KALK1_CONTAINER_REGISTRY") {
                sh "docker build -t kalk1-on.azurecr.io/${projectName}:${version} ."
                sh "docker push kalk1-on.azurecr.io/${projectName}:${version}"
            }
        }

        if (env.BRANCH_NAME == "master") {
            docker.withRegistry("https://kalk1-on.azurecr.io", "KALK1_CONTAINER_REGISTRY") {
                sh "docker build -t kalk1-on.azurecr.io/${projectName}:prod.${version} ."
                sh "docker push kalk1-on.azurecr.io/${projectName}:prod.${version}"
            }
        }
    }
    // stage('Deployment') {
    //     if (env.BRANCH_NAME  == "develop") {
    //         sh"""#!/bin/bash -xe
    //             kubectl config use-context egonedev
    //             kubectl set image deployment/${projectName} ${projectName}=kalk1-on.azurecr.io/${projectName}:${version}
    //         """
    //     }

    //     if (env.BRANCH_NAME == "master") {
    //         sh"""#!/bin/bash -xe
    //             kubectl config use-context egone-kube
    //             kubectl set image deployment/${projectName} ${projectName}=kalk1-on.azurecr.io/${projectName}:prod.${version}
    //         """
    //     }
    // }
}
