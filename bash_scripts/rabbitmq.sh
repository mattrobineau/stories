#!/bin/bash
# Setup rabbit mq with dotnetsignals settings
# !WARNING! change password for user before running the script

echo "deb http://www.rabbitmq.com/debian/ testing main" | sudo tee /etc/apt/sources.list.d/rabbitmq.list
wget -O- https://www.rabbitmq.com/rabbitmq-release-signing-key.asc | sudo apt-key add -
sudo apt-get update
sudo apt-get install rabbitmq-server

sudo rabbitmqctl add_user dotnetsignals password # change password to actual password
sudo rabbitmqctl set_user_tags dotnetsignals administrator
sudo rabbitmqctl set_permissions -p / dotnetsignals ".*" ".*" ".*"

./rabbitmqadmin declare exchange name=stories.ranking type=direct
./rabbitmqadmin declare queue name=stories.queues.stories durable=true
./rabbitmqadmin declare queue name=stories.queues.comments durable=true
./rabbitmqadmin declare binding source="stories.ranking" destination_type="queue" destination="stories.queues.stories" routing_key="story"
./rabbitmqadmin declare binding source="stories.ranking" destination_type="queue" destination="stories.queues.comments" routing_key="comment"
