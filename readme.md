# Infra:

## Image registry service
Run a containerised docker image registry service for allowing local cluster to access it:
```bash
cd docker-registry
docker-compose up -d
``` 

## Opensearch service with dashboard
This will be the sink where fluentd will forward logs. Logs will be searchable here based on multiple criteria. More info about opensearch can be found here: https://www.opensearch.org/downloads.html#docker-compose

```bash
cd opensearch
docker-compose up -d
```
The opensearch host runs at http://localhost:9200 and opensearch dashboard can be opened on browser at http://localhost:5601

## Cluser setup
This demo uses [minikube](https://minikube.sigs.k8s.io/docs/start/).

Add insecure local docker registry service to minikube config.
```bash
minikube config set insecure-registry minikube.host.internal:5000
```
Run a local cluster.
```bash
minikube start
```

# Fluentd deployment:
https://docs.fluentd.org/container-deployment/kubernetes

Deploy fluentd as a daemonset to the cluster so that an instance of it runs in every node on the cluster. We are using bitnami chart which uses [forwarder-aggregator pattern](https://fluentbit.io/blog/2020/12/03/common-architecture-patterns-with-fluentd-and-fluent-bit/).
```bash
helm repo add bitnami https://charts.bitnami.com/bitnami
helm repo update
cd fluentd
helm install fluentd-release -f values.yaml bitnami/fluentd
# Read more about the installation in the Fluentd packaged by Bitnami Chart Github repository
```

# App deployment:

Steps:
1. Build, tag and push sampleWebAPI docker image to the docker registry service:
```bash
cd sampleWebAPI
docker build --no-cache -t samplewebapi:latest  -f SampleWebAPI/Dockerfile --build-arg projectName=SampleWebAPI .
docker tag samplewebapi:latest localhost:5000/samplewebapi:latest
docker push localhost:5000/samplewebapi:latest
```

2. Deploy sampleWebAPI in local cluster using its helm chart:
```bash
helm install samplewebapi ./tools/chart/samplewebapi
```