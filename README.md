# AkkaSingletonResilientAndLoadBalancedConsumers
Use AkkaSingleton to periodically de/activate consumers in a cluster using round-robin to distribute load.
That way when a node joins it will share the load as some of its consumers will be activated and when leaving its load will be offloaded to other nodes.
This is to achieve having multiple single consumers but distributed in different nodes.
