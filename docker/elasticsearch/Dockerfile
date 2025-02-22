FROM docker.elastic.co/elasticsearch/elasticsearch:8.17.1

# Add new kibana user
RUN bin/elasticsearch-users useradd kibana_user -p kibana -r kibana_system