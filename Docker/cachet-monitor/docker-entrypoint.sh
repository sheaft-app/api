#!/bin/sh
set -e


if [ -n "$CONFIG" ]; then
	echo "Found configuration variable, will write it to /etc/cachet-monitor.yaml"
	echo "$CONFIG" > /etc/cachet-monitor.yaml
elif [ ! -f /etc/cachet-monitor.yaml ]; then
        echo "Please provide configuration in CONFIG variable or write it in /etc/cachet-monitor.yaml"
        exit 1	
fi

exec "$@"