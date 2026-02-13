param location string = resourceGroup().location

param namePrefix string
param adminUsername string
param adminPublicKey string
param addressSpace string = '10.10.0.0/16'
param subnetPrefix string = '10.10.1.0/24'

var vnetName = '${namePrefix}-vnet'
var subnetName = '${namePrefix}-subnet'
var nsgName  = '${namePrefix}-nsg'
var pipName  = '${namePrefix}-ip'
var nicName  = '${namePrefix}-nic'
var vmName   = '${namePrefix}-vm'

// network 
resource vnet 'Microsoft.Network/virtualNetworks@2024-07-01' = {
  name: vnetName
  location: location
  properties: {
    addressSpace: { addressPrefixes: [ addressSpace ] }
    subnets: [{ name: subnetName, properties: { addressPrefix: subnetPrefix } }]
  }
}

// firewall
resource nsg 'Microsoft.Network/networkSecurityGroups@2024-07-01' = {
  name: nsgName
  location: location
  properties: {
    securityRules: [
      {
        name: 'SSH'
        properties: {
          protocol: 'TCP'
          sourcePortRange: '*'
          destinationPortRange: '22'
          sourceAddressPrefix: '*'
          destinationAddressPrefix: '*'
          access: 'Allow'
          priority: 300
          direction: 'Inbound'
        }
      }
      {
        name: 'Kestrel'
        properties: {
          protocol: 'TCP'
          sourcePortRange: '*'
          destinationPortRange: '5000'
          sourceAddressPrefix: '*'
          destinationAddressPrefix: '*'
          access: 'Allow'
          priority: 330
          direction: 'Inbound'
        }
      }
    ]
  }
}

// ipaddress
resource pip 'Microsoft.Network/publicIPAddresses@2024-07-01' = {
  name: pipName
  location: location
  sku: { name: 'Standard', tier: 'Regional' }
  properties: {
    publicIPAddressVersion: 'IPv4'
    publicIPAllocationMethod: 'Static'
  }
}

resource nic 'Microsoft.Network/networkInterfaces@2024-07-01' = {
  name: nicName
  location: location
  properties: {
    ipConfigurations: [{
      name: 'ipconfig1'
      properties: {
        subnet: { id: resourceId('Microsoft.Network/virtualNetworks/subnets', vnet.name, subnetName) }
        publicIPAddress: { id: pip.id }
        privateIPAllocationMethod: 'Dynamic'
        primary: true
      }
    }]
    networkSecurityGroup: { id: nsg.id }
  }
}

resource vm 'Microsoft.Compute/virtualMachines@2024-11-01' = {
  name: vmName
  location: location
  properties: {
    hardwareProfile: {
      vmSize: 'Standard_B2ts_v2'
    }
    storageProfile: {
      imageReference: {
        publisher: 'canonical'
        offer: 'ubuntu-24_04-lts'
        sku: 'server'
        version: 'latest'
      }
      osDisk: {
        name: '${vmName}_Disk'
        createOption: 'FromImage'
        caching: 'ReadWrite'
        managedDisk: {
          storageAccountType: 'StandardSSD_LRS'
        }
        deleteOption: 'Delete'
        diskSizeGB: 30
      }
    }
    osProfile: {
      computerName: vmName
      adminUsername: adminUsername
      linuxConfiguration: {
        disablePasswordAuthentication: true
        ssh: {
          publicKeys: [{ path: '/home/${adminUsername}/.ssh/authorized_keys', keyData: adminPublicKey }]
        }
        provisionVMAgent: true
      }
      allowExtensionOperations: true
    }
    networkProfile: {
      networkInterfaces: [
        {
          id: nic.id
        }
      ]
    }
  }
}

output publicIpAddress string = pip.properties.ipAddress
