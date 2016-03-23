<#
.Synopsis
   Get Signed Driver information for remote computer
.DESCRIPTION
   Retrieve Signed Driver information from remote or local computer by using the WMI service.
.EXAMPLE
   Get-DriverInfo -Computername $computer
.EXAMPLE
   Get-DriverInfo -ComputerName (Get-Content -Path $path)
#>
function Get-DriverInfo
{
    [CmdletBinding()]
    Param
    (
        # Computer Name
        # Can be used for multiple computers
        [Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true,Position=0)]
        [string[]]$ComputerName
    )

    Process
    {
        foreach($pc in $ComputerName)
        {

            Write-Verbose "Testing Connection to $pc"
            

            if(Test-Connection $pc -Count 1 -Quiet)
            {
                Try
                {

                        Write-Verbose "Gathering Data from $pc"
                        
                
                        $a = "<style>"
                        $a = $a + "BODY{background-color:#fff;}"
                        $a = $a + "H2{border-right:30px solid #fff;border-left:30px solid #fff;color:#669;padding:12px 2px 0;vertical-align:top;}"
                        $a = $a + "TABLE{font-family:'Lucida Sans Unicode', 'Lucida Grande', Sans-Serif; font-size:12px;width:80%;text-align:left;border-collapse:collapse;margin: 20px auto;border-spacing:0;vertical-align:baseline;}"
                        $a = $a + "TABLE TR:hover td{color:#339;background:#d0dafd;}"
                        $a = $a + "TABLE TH{font-weight:normal;font-size:14px;border-bottom:2px solid #6678b1;border-right:30px solid #fff;border-left:30px solid #fff;color:#039;padding: 8px 2px;}"
                        $a = $a + "TABLE TD{border-right:30px solid #fff;border-left:30px solid #fff;color:#669;padding:12px 2px 0;vertical-align:top;}"
                        $a = $a + "</style>"

                        $data = Get-WmiObject Win32_ComputerSystem -ComputerName $pc | select *
                        $machine = $data.Name
                        $user = $data.UserName

                        $date = Get-Date -Format "dd/MM/yyyy"

                        Write-Verbose "Generating Report for $machine"
                       

                        $drivers =  Get-WmiObject win32_PnPSignedDriver -ComputerName $pc -Filter "DeviceClass != 'LEGACYDRIVER' and Description IS NOT NULL " | select DeviceClass, Description, DriverVersion, @{Expression={[management.managementDateTimeConverter]::ToDateTime($_.DriverDate)};Label="DriverDate"}
                        $drivers | ConvertTo-HTML -head $a -Body "<h2>$machine - $user - $date Drivers </h2>" | Out-File C:\Temp\$pc-Drivers.htm
                
                        Write-Verbose "Saved to C:\Temp\"
                        Write-Output "Saved to C:\Temp\"                  
                              
                }
                Catch
                {
                    Write-Output "Machine $pc not online or PowerShell is unable to read it"
                    Write-Output "$($Error[0])"
                }

            }
            else
            {
                Write-Output "Machine not online"
            }

            Write-Verbose "Report complete for $pc"
            
        }
    }

}

<#
.Synopsis
   Get EventLog information for remote computer
.DESCRIPTION
   Retrieve EventLog information from remote or local computer by using the WMI service.
.EXAMPLE
   Get-EventLogInfo -Computername $computer
.EXAMPLE
   Get-EventLogInfo -ComputerName (Get-Content -Path $path)
#>
function Get-EventLogInfo
{
    [CmdletBinding()]
    Param
    (
        # Computer Name
        [Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true,Position=0)]
        [string[]]$computername
    )

    Process
    {
        foreach($pc in $computername)
        {

            Write-Verbose "Testing Connection to $pc"
            

            if(Test-Connection $pc -Count 1 -Quiet)
            {
                Try
                {
                   
                    Write-Verbose "Gathering Data from $pc"
                    

                        $a = "<style>"
                        $a = $a + "BODY{background-color:#fff;}"
                        $a = $a + "H2{border-right:30px solid #fff;border-left:30px solid #fff;color:#669;padding:12px 2px 0;vertical-align:top;}"
                        $a = $a + "TABLE{font-family:'Lucida Sans Unicode', 'Lucida Grande', Sans-Serif; font-size:12px;width:98%;text-align:left;border-collapse:collapse;margin: 20px auto;border-spacing:0;vertical-align:baseline;}"
                        $a = $a + "TABLE TR:hover td{color:#339;background:#d0dafd;}"
                        $a = $a + "TABLE TH{font-weight:normal;font-size:14px;border-bottom:2px solid #6678b1;border-right:30px solid #fff;border-left:30px solid #fff;color:#039;padding: 8px 2px;}"
                        $a = $a + "TABLE TD{border-right:30px solid #fff;border-left:30px solid #fff;color:#669;padding:12px 2px 0;vertical-align:top;}"
                        $a = $a + "</style>"

                        $data = Get-WmiObject Win32_ComputerSystem -ComputerName $pc | select *
                        $machine = $data.Name
                        $user = $data.UserName

                        $date = Get-Date -Format "dd/MM/yyyy"

                        Write-Verbose "Generating Reports"

                        Get-EventLog -LogName System -ComputerName $pc -EntryType Error, Warning -Newest 50 | select EventID, MachineName, Index, EntryType, Message, Source, InstanceId, TimeGenerated, UserName | ConvertTo-HTML -head $a -Body "<h2>$machine - $user - $date EventLog-System </h2>" | Out-File C:\Temp\$pc-EventLog-System.htm
                        Get-EventLog -LogName Application -ComputerName $pc -EntryType Error, Warning -Newest 50 | select EventID, MachineName, Index, EntryType, Message, Source, InstanceId, TimeGenerated, UserName | ConvertTo-HTML -head $a -Body "<h2>$machine - $user - $date EventLog-Application </h2>" | Out-File C:\Temp\$pc-EventLog-Application.htm

                }
                Catch
                {
                    Write-Output "Machine $pc not online or PowerShell is unable to read it"
                    Write-Output "$($Error[0])"
                }
            }
            else
            {
                Write-Output "Machine not online"
            }

            Write-Verbose "Report complete for $pc"
            
        }
    }

}

<#
.Synopsis
   Get Service information for remote computer
.DESCRIPTION
   Retrieve Service information from remote or local computer by using the WMI service.
.EXAMPLE
   Get-ServiceInfo -Computername $computer
.EXAMPLE
   Get-ServiceInfo -ComputerName (Get-Content -Path $path)
#>
function Get-ServiceInfo
{
    [CmdletBinding()]
    Param
    (
        # Computer Name
        [Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true,Position=0)]
        [string[]]$computername
    )

    Process
    {
        foreach($pc in $computername)
        {

            Write-Verbose "Testing Machine"

            if(Test-Connection $pc -Count 1 -Quiet)
            {
                Try
                {
                    Write-Verbose "Gathering Data"

                    $a = "<style>"
                    $a = $a + "BODY{background-color:#fff;}"
                    $a = $a + "H2{border-right:30px solid #fff;border-left:30px solid #fff;color:#669;padding:12px 2px 0;vertical-align:top;}"
                    $a = $a + "TABLE{font-family:'Lucida Sans Unicode', 'Lucida Grande', Sans-Serif; font-size:12px;width:80%;text-align:left;border-collapse:collapse;margin: 20px auto;border-spacing:0;vertical-align:baseline;}"
                    $a = $a + "TABLE TR:hover td{color:#339;background:#d0dafd;}"
                    $a = $a + "TABLE TH{font-weight:normal;font-size:14px;border-bottom:2px solid #6678b1;border-right:30px solid #fff;border-left:30px solid #fff;color:#039;padding: 8px 2px;}"
                    $a = $a + "TABLE TD{border-right:30px solid #fff;border-left:30px solid #fff;color:#669;padding:12px 2px 0;vertical-align:top;}"
                    $a = $a + "</style>"

                    $data = Get-WmiObject Win32_ComputerSystem -ComputerName $pc | select *
                    $machine = $data.Name
                    $user = $data.UserName

                    $date = Get-Date -Format "dd/MM/yyyy"

                    Write-Verbose "Generating Report for $machine"

                    Get-Service -ComputerName $pc | Select-Object Status, Name, DisplayName | Sort-Object Status -Descending | ConvertTo-HTML -head $a -Body "<h2>$machine - $user - $date Services </h2>" | Out-File C:\Temp\$pc-Services.htm

                }
                Catch
                {
                    Write-Output "Machine not online or PowerShell is unable to read it"
                    Write-Output "$($Error[0])"
                }
            }
            else
            {
                Write-Output "Machine not online"
            }
        }
    }

}


<#
.Synopsis
   Get Group Policy Report information for remote user.
.DESCRIPTION
   Retrieve Group Policy Report information for a logged in user on a remote computer.
.EXAMPLE
   Get-GPReport -Computername $computer
#>
function Get-GPReport
{
    [CmdletBinding()]
    Param
    (
        # Computer Name
        # Can be used for multiple computers
        [Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true,Position=0)]
        [string[]]$ComputerName
    )

    Process
    {
        Write-Verbose "Testing Connection to $ComputerName"
        
        
        if(Test-Connection $ComputerName -Count 1 -Quiet)
        {
            Try
            {
                Write-Verbose "Gathering Data from $ComputerName"
                Write-Output "Gathering Data from $ComputerName"

                $user = Get-WmiObject -Class win32_computersystem -ComputerName $ComputerName | select -expand username
                gpresult /S $ComputerName /user $user /H C:\Temp\GPReport.html /f

                Write-Verbose "Report generated for $ComputerName $user"

            }
            Catch
            {
                Write-Output "Machine not online or PowerShell is unable to read it"
                Write-Output "$($Error[0])"
            }
        }
        else
        {
            Write-Output "Machine not online"
        }

        Write-Verbose "Report complete for $ComputerName"
        
    }
}


<#
.Synopsis
   Get Windows information for remote computer
.DESCRIPTION
   Retrieve Windows information from remote or local computer by using the WMI service.
.EXAMPLE
   Get-WindowsInfo -Computername $computer
.EXAMPLE
   Get-WindowsInfo -ComputerName (Get-Content -Path $path)
#>
function Get-WindowsInfo
{
    [CmdletBinding()]
    Param
    (
        # Computer Name
        # Can be used for multiple computers
        [Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true,Position=0)]
        [string[]]$ComputerName
    )

    Process
    {
        foreach($pc in $ComputerName)
        {

            Write-Verbose "Testing Connection to $pc"
            

            if(Test-Connection $pc -Count 1 -Quiet)
            {
                Try
                {
                    Write-Verbose "Gathering Data from $pc"
                    

                    $windows = Get-WmiObject -Class Win32_OperatingSystem -ComputerName $pc -ErrorAction Stop

                    foreach($win in $windows)
                    {
                            $myobj = @{

                                'Windows Type' = $windows.Caption
                                'Install Date' =  [management.managementDateTimeConverter]::ToDateTime($windows.InstallDate)
                                'LastBoot UpTime' =  [management.managementDateTimeConverter]::ToDateTime($windows.LastBootUpTime)
                                'OS Architecture' = $windows.OSArchitecture
                                'Serial Number' = $windows.SerialNumber
                                'System Drive' = $windows.SystemDrive
                                'System Directory' = $windows.SystemDirectory
                            }

                        Write-Output (New-Object -TypeName PSObject -Property $myobj)

                    }

                }
                Catch
                {
                    Write-Output "`nMachine not online or PowerShell is unable to read it"
                    Write-Output "$($Error[0])"
                }
            }
            else
            {
                Write-Output "Machine not online"
            }

            Write-Verbose "Report complete for $pc"
            
        }
    }

}


<#
.Synopsis
   Get Network Adapter and Driver information for remote computer
.DESCRIPTION
   Retrieve Network Adapter and Driver information from remote or local computer by using the WMI service.
.EXAMPLE
   Get-NICInfo -Computername $computer
.EXAMPLE
   Get-NICInfo -ComputerName (Get-Content -Path $path)
#>
function Get-NICInfo
{
    [CmdletBinding()]
    Param
    (
        # Computer Name
        # Can be used for multiple computers
        [Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true,Position=0)]
        [string[]]$ComputerName
    )

    Process
    {
        foreach ($pc in $ComputerName)
        {

            Write-Verbose "Testing Connection to $pc"
            
        
            if(Test-Connection $pc -Count 1 -Quiet)
            {
                Try
                {
                    Write-Verbose "Gathering Data from $pc"
                    
                
                    $nicprops = Get-WmiObject Win32_NetworkAdapter -ComputerName $pc -Filter "PhysicalAdapter = 'True'" | Where-Object { $_.Name -notlike "*Check*" -and $_.Manufacturer -notlike "*Microsoft*" -and $_.Manufacturer -notlike "*VMware*"} | select *

                    $nicdriverprops = Get-WmiObject Win32_PnPSignedDriver -Filter "DeviceClass = 'NET'" -ComputerName $pc | Where-Object { $_.Description -notlike "*Microsoft*" -and $_.Description -notlike "*Check*" -and $_.Description -notlike "*WAN*" -and $_.Description -notlike "*RAS*" } | select *

                    Write-Output "----------Physical Adapters---------"
                
                    foreach ($nicprop in $nicprops)
                    {
                       $netadapt = @{
                                
                            'Name' = $nicprop.Name
                            'Adapter Type' = $nicprop.AdapterType
                            'Power Management Supported' = $nicprop.PowerManagementSupported
                            'GUID' = $nicprop.GUID
                            'Index' = $nicprop.Index
                            'Interface Index' = $nicprop.InterfaceIndex
                            'MAC Address' = $nicprop.MACAddress
                            'Manufacturer' = $nicprop.Manufacturer
                            'Net Connection ID' = $nicprop.NetConnectionID
                            'Net Connection Status' = switch ($nicprop.NetConnectionStatus)
                                                        {
                                                            0 {"Disconnected"} 
                                                            1 {"Connecting"} 
                                                            2 {"Connected"} 
                                                            3 {"Disconnecting"} 
                                                            4 {"Hardware not present"} 
                                                            5 {"Hardware disabled"} 
                                                            6 {"Hardware malfunction"} 
                                                            7 {"Media disconnected"} 
                                                            8 {"Authenticating"} 
                                                            9 {"Authentication succeeded"} 
                                                            10 {"Authentication failed"} 
                                                            11 {"Invalid address"} 
                                                            12 {"Credentials required"}
                                                        }
                            'Net Enabled' = $nicprop.NetEnabled
                            'PNP Device ID' = $nicprop.PNPDeviceID
                            'Speed (MB/s)' = ($nicprop.Speed /1MB -as [int])
                            'Time Of Last Reset' = [management.managementDateTimeConverter]::ToDateTime($nicprop.TimeOfLastReset)          
                        
                       }
                   
                       Write-Output (New-Object -TypeName PSObject -Property $netadapt ) 
                    }

                    Write-Output "----------Physical Adapters Drivers---------`n"

                    foreach ($nicdriverprop in $nicdriverprops)
                    {
                       $netadaptdriver = @{
                        
                            'Device Name' = $nicdriverprop.DeviceName
                            'Class Guid' = $nicdriverprop.ClassGuid
                            'Compat ID' = $nicdriverprop.CompatID
                            'Device ID' = $nicdriverprop.DeviceID
                            'Driver Date' = [management.managementDateTimeConverter]::ToDateTime($nicdriverprop.DriverDate)
                            'Driver Version' = $nicdriverprop.DriverVersion
                            'Driver Provider Name' = $nicdriverprop.DriverProviderName
                            'Inf Name' = $nicdriverprop.InfName
                            'Is Signed' = $nicdriverprop.IsSigned
                            'Location' = $nicdriverprop.Location        
                               
                        
                       }
                   
                       Write-Output (New-Object -TypeName PSObject -Property $netadaptdriver ) 
                    }

                }
                Catch
                {
                    Write-Output "Machine $pc not online or PowerShell is unable to read it"
                    Write-Output "$($Error[0])"
                }
            }
            else
            {
                Write-Output "Machine not online"
            }

            Write-Verbose "Report complete for $pc"
            
        }

    }
}


<#
.Synopsis
   Get BIOS information on remote computers
.DESCRIPTION
   Retrieve BIOS information on remote or local computer by using the WMI service. 
.EXAMPLE
   Get-BiosInfo -ComputerName $computer
.EXAMPLE
   Get-BiosInfo -ComputerName (Get-Content -Path $path)
#>
function Get-BiosInfo
{
    [CmdletBinding()]
    Param
    (
        # Computer Name
        # Can be used for multiple computers
        [Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true,Position=0)]
        [string[]]$ComputerName
    )

    Process
    {
        foreach($pc in $ComputerName)
        {

            Write-Verbose "Testing Connection to $pc"
            
        
            if(Test-Connection $pc -Count 1 -Quiet)
            {
                Try
                {
                    Write-Verbose "Gathering Data from $pc"
                    
                
                    $bioses = Get-WmiObject -Class Win32_Bios -ComputerName $pc

                    foreach ($bios in $bioses)
                    {
                       $myobj = @{

                            'Status' = $bios.Status
                            'Name' = $bios.Name
                            'Current Language' = $bios.CurrentLanguage
                            'Release Date' = [management.managementDateTimeConverter]::ToDateTime($bios.ReleaseDate)
                            'SMB Version' = $bios.SMBIOSBIOSVersion
                            'Serial Number' = $bios.SerialNumber
                            'Version' = $bios.Version

                       }
                   
                       Write-Output (New-Object -TypeName PSObject -Property $myobj)
                    }

                }
                Catch
                {
                    Write-Output "Machine $pc not online or PowerShell is unable to read it"
                    Write-Output "$($Error[0])"
                }
            }
            else
            {
                Write-Output "Machine not online"
            }

            Write-Verbose "Report complete for $pc"
            
        }
    }
}


<#
.Synopsis
.Synopsis
   Get Connected Network Adapter information for remote computer
.DESCRIPTION
   Retrieve Connected Network Adapter information from remote or local computer by using the WMI service.
.EXAMPLE
   Get-ConnectedNic -Computername $computer
.EXAMPLE
   Get-ConnectedNic -ComputerName (Get-Content -Path $path)
#>
function Get-ConnectedNic
{
    [CmdletBinding()]
    Param
    (
        # Computer Name
        # Can be used for multiple computers
        [Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true,Position=0)]
        [string[]]$ComputerName
    )

    Process
    {
        foreach($pc in $ComputerName)
        {

            Write-Verbose "Testing Connection to $pc"
            
        
            if(Test-Connection $pc -Count 1 -Quiet)
            {
                Try
                {
                   foreach($pc in $pc)
                
                   { 
                        Write-Verbose "Gathering Data from $pc"
                        
                
                        $connectednics = Get-WmiObject -Class Win32_NetworkAdapterConfiguration -ComputerName $pc -Filter "ipenabled = 'true'" | select *

                        foreach ($connectednic in $connectednics)
                        {
                           $myobj = @{

                                    'Description' = $connectednic.Description
                                    'IP Enabled' = $connectednic.IPEnabled
                                    'IP Address' = ($connectednic.IPAddress -join "`n")
                                    'IP Filter Security Enabled' = $connectednic.IPFilterSecurityEnabled
                                    'DHCP Enabled' = $connectednic.DHCPEnabled
                                    'DHCP Server' = $connectednic.DHCPServer
                                    'DHCP Lease Obtained' = [management.managementDateTimeConverter]::ToDateTime($connectednic.DHCPLeaseObtained)
                                    'DHCP Lease Expires' = [management.managementDateTimeConverter]::ToDateTime($connectednic.DHCPLeaseExpires)
                                    'DNS Domain' = $connectednic.DNSDomain
                                    'DNS Domain Suffix Search Order' = ($connectednic.DNSDomainSuffixSearchOrder -join "`n")
                                    'DNS Enabled For WINS Resolution' = $connectednic.DNSEnabledForWINSResolution
                                    'DNS Server Search Order' = ($connectednic.DNSServerSearchOrder -join "`n" )
                                    'Domain DNS Registration Enabled' = $connectednic.DomainDNSRegistrationEnabled
                                    'Full DNS Registration Enabled' = $connectednic.FullDNSRegistrationEnabled
                                    'IP Connection Metric' = $connectednic.IPConnectionMetric
                                    'WINS Enable LM Hosts Lookup' = $connectednic.WINSEnableLMHostsLookup
                                    'WINS Primary Server' = $connectednic.WINSPrimaryServer
                                    'WINS Secondary Server' = $connectednic.WINSSecondaryServer
                                    'Default IP Gateway' = ($connectednic.DefaultIPGateway -join "`n")
                                    'IP Subnet' = ($connectednic.IPSubnet -join "`n")
                                    'MAC Address' = $connectednic.MACAddress
                                    'Tcp ip Netbios Options' = $connectednic.TcpipNetbiosOptions
                                    'Tcp Window Size' = $connectednic.TcpWindowSize
                                    'Setting ID' = $connectednic.SettingID
                        
                           }
                   
                           Write-Output (New-Object -TypeName PSObject -Property $myobj ) 
                        }

                    }

                }
                Catch
                {
                    Write-Output "Machine $pc not online or PowerShell is unable to read it"
                    Write-Output "$($Error[0])"
                }
            }
            else
            {
                Write-Output "Machine not online"
            }

            Write-Verbose "Report complete for $pc"
            
        }
    }
}


<#
.Synopsis
   Get Hard Disk information for remote computer
.DESCRIPTION
   Retrieve Hard Disk information from remote or local computer by using the WMI service.
.EXAMPLE
   Get-HDDInof -Computername $computer
.EXAMPLE
   Get-HDDInfo -ComputerName (Get-Content -Path $path)
#>
function Get-HDDInfo
{
    [CmdletBinding()]
    Param
    (
        # Computer Name
        # Can be used for multiple computers
        [Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true,Position=0)]
        [string[]]$ComputerName
    )

    Process
    {

        foreach($pc in $ComputerName)
        {

            Write-Verbose "Testing Connection to $pc"
            
        
            if(Test-Connection $pc -Count 1 -Quiet)
            {
                Try
                {
                    Write-Verbose "Gathering Data from $pc"
                    
                
                    $harddrives = Get-WmiObject -Class win32_DiskDrive -ComputerName $pc

                    foreach ($harddrive in $harddrives)
                    {
                       $myobj = @{

                            'Status' = $harddrive.Status
                            'Index' = $harddrive.Index
                            'Device ID' = $harddrive.DeviceID
                            'Name' = $harddrive.Name
                            'Model' = $harddrive.Model
                            'Serial Number' = ($harddrive.SerialNumber).Trim()
                            'Partitions' = $harddrive.Partitions
                            'Bytes Per Sector' = $harddrive.BytesPerSector
                            'Interface Type' = $harddrive.InterfaceType
                            'Sectors Per Track' = $harddrive.SectorsPerTrack
                            'Size (GB)' = ($harddrive.Size / 1GB -as [int])
                            'Total Cylinders' = $harddrive.TotalCylinders
                            'Total Heads' = $harddrive.TotalHeads
                            'Total Sectors' = $harddrive.TotalSectors
                            'Total Tracks' = $harddrive.TotalTracks
                            'Tracks Per Cylinder' = $harddrive.TracksPerCylinder
                            'Media Loaded' = $harddrive.MediaLoaded
                            'Media Type' = $harddrive.MediaType
                            'Signature' = $harddrive.Signature

                       }
                   
                       Write-Output (New-Object -TypeName PSObject -Property $myobj ) 
                    }

                }
                Catch
                {
                    Write-Output "Machine not online or PowerShell is unable to read it"
                    Write-Output "$($Error[0])"
                }
            }
            else
            {
                Write-Output "Machine not online"
            }

            Write-Verbose "Report complete for $pc"
            
        }
    }
}


<#
.Synopsis
   Get Memory information for remote computer
.DESCRIPTION
   Retrieve Memory information from remote or local computer by using the WMI service.
.EXAMPLE
   Get-MemoryInfo -Computername $computer
.EXAMPLE
   Get-MemoryInfo -ComputerName (Get-Content -Path $path)
#>
function Get-MemoryInfo
{
    [CmdletBinding()]
    Param
    (
        # Computer Name
        # Can be used for multiple computers
        [Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true,Position=0)]
        [string[]]$ComputerName
    )

    Process
    {
        foreach($pc in $ComputerName)
        {

            Write-Verbose "Testing Connection to $pc"
            
        
            if(Test-Connection $pc -Count 1 -Quiet)
            {
                Try
                {
                    Write-Verbose "Gathering Data from $pc"
                    
                
                    $memories = Get-WmiObject -Class Win32_PhysicalMemory -ComputerName $pc

                    foreach ($memory in $memories)
                    {
                       $myobj = @{
                        
                            'Manufacturer' = $memory.Manufacturer
                            'Caption' = $memory.Caption
                            'Tag' = $memory.Tag
                            'Capacity' = ($memory.Capacity /1GB -as [int])
                            'Bank Label' = $memory.BankLabel
                            'Device Locator' = $memory.DeviceLocator
                            'Form Factor' = switch ($memory.FormFactor)
                                                {
                                                    0 {"Unknown"}
                                                    1 {"Other"}
                                                    2 {"SIP"}
                                                    3 {"DIP"}
                                                    4 {"ZIP"}
                                                    5 {"SOJ"}
                                                    6 {"Proprietary"}
                                                    7 {"SIMM"}
                                                    8 {"DIMM"}
                                                    9 {"TSOP"}
                                                    10 {"PGA"}
                                                    11 {"RIMM"}
                                                    12 {"SODIMM"}
                                                    13 {"SRIMM"}
                                                    14 {"SMD"}
                                                    15 {"SSMP"}
                                                    16 {"QFP"}
                                                    17 {"TQFP"}
                                                    18 {"SOIC"}
                                                    19 {"LCC"}
                                                    20 {"PLCC"}
                                                    21 {"BGA"}
                                                    22 {"FPBGA"}
                                                    23 {"LGA"}
                                                    default {"Could not determine type of installed RAM"}
                                                }
                            'Interleave Data Depth' = $memory.InterleaveDataDepth
                            'Interleave Position' = switch ($memory.InterleavePosition)
                                                        {
                                                            0 {"Noninterleaved"}
                                                            1 {"First position"}
                                                            2 {"Second position"}
                                                            default {"Could not determine"}
                                                        }
                            'Memory Type' = switch ($memory.MemoryType)
                                                {
                                                0 {"Unknown"}
                                                1 {"Other"}
                                                2 {"DRAM"}
                                                3 {"Synchronous DRAM"}
                                                4 {"Cache DRAM"}
                                                5 {"EDO"}
                                                6 {"EDRAM"}
                                                7 {"VRAM"}
                                                8 {"SRAM"}
                                                9 {"RAM"}
                                                10 {"ROM"}
                                                11 {"Flash"}
                                                12 {"EEPROM"}
                                                13 {"FEPROM"}
                                                14 {"EPROM"}
                                                15 {"CDRAM"}
                                                16 {"3DRAM"}
                                                17 {"SDRAM"}
                                                18 {"SGRAM"}
                                                19 {"RDRAM"}
                                                20 {"DDR"}
                                                21 {"DDR2"}
                                                22 {"DDR2 FB-DIMM"}
                                                24 {"DDR3"}
                                                25 {"FBD2"}
                                                default {"Could not determine type of installed RAM"}
                                                }
                            'Part Number' = $memory.PartNumber
                            'Serial Number' = $memory.SerialNumber
                            'Speed (MHz)' = $memory.Speed
                            'Total Width' = $memory.TotalWidth
                            'Type Detail' = switch ($memory.TypeDetail)
                                                {
                                                    1 {"Reserved"} 
                                                    2 {"Other"} 
                                                    4 {"Unknown"} 
                                                    8 {"Fast-paged"} 
                                                    16 {"Static column"} 
                                                    32 {"Pseudo-static"} 
                                                    64 {"RAMBUS"} 
                                                    128 {"Synchronous"} 
                                                    256 {"CMOS"} 
                                                    512 {"EDO"} 
                                                    1024 {"Window DRAM"}
                                                    2048 {"Cache DRAM"}
                                                    4096 {"Nonvolatile"}
                                                }
                       }
                   
                       Write-Output (New-Object -TypeName PSObject -Property $myobj ) 
                    }

                }
                Catch
                {
                    Write-Output "Machine $pc not online or PowerShell is unable to read it"
                    Write-Output "$($Error[0])"
                }
            }
            else
            {
                Write-Output "Machine not online"
            }

            Write-Verbose "Report complete for $pc"
            
        }
        
    }
}


<#
.Synopsis
   Get Model infromation for remote computer
.DESCRIPTION
   Retrieve Model information from remote or local computer by using the WMI service.
.EXAMPLE
   Get-ModelInfo -ComputerName $pc
.EXAMPLE
   Get-ModelInfo -ComputerName (Get-Content -Path $path)
#>
function Get-ModelInfo
{
    [CmdletBinding()]
    Param
    (
        # Computer Name
        # Can be used for multiple computers
        [Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true,Position=0)]
        [string[]]$ComputerName
    )

    Process
    {
        foreach($pc in $ComputerName)
        {

            Write-Verbose "Testing Connection to $pc"
            
        
            if(Test-Connection $pc -Count 1 -Quiet)
            {
                Try
                {
                    Write-Verbose "Gathering Data from $pc"
                    
                
                    $models = Get-WmiObject -Class win32_computersystem -ComputerName $pc

                    foreach ($model in $models)
                    {
                       $myobj = @{
                        
                            'Name' = $model.Name
                            'Status' = $model.Status
                            'Domain' = $model.Domain
                            'Manufacturer' = $model.Manufacturer
                            'Model' = $model.Model
                            'System Type' =  switch ($model.PCSystemType)
                                                  {
                                                    0 {"Unspecified"} 
                                                    1 {"Desktop"} 
                                                    2 {"Laptop"} 
                                                    3 {"Workstation"} 
                                                    4 {"Enterprise Server"} 
                                                    5 {"Small Office and Home Office (SOHO) Server"} 
                                                    6 {"Appliance PC"} 
                                                    7 {"Performance Server"} 
                                                    8 {"Maximum"}
                                                  }#End of Switch
                            'Installed RAM (GB)' = ($model.TotalPhysicalMemory / 1GB -as [int])
                            'CPU(s)' = $model.NumberOfProcessors
                            'Cores' = $model.NumberOfLogicalProcessors

                       }#End of custom object
                   
                       Write-Output (New-Object -TypeName PSObject -Property $myobj ) 
                    }#End of foreach

                }#End of try
                Catch
                {
                    Write-Output "Machine $pc not online or PowerShell is unable to read it"
                    Write-Output "$($Error[0])"
                }
            }
            else
            {
                Write-Output "Machine not online"
            }

            
            Write-Verbose "Report Complete for $pc"
        }#End of foreach machine

    }
}


<#
.Synopsis
   Get Partition information on remote computers
.DESCRIPTION
   Retrieve Partition information on remote or local computer by using the WMI service.
.EXAMPLE
   Get-PartitionInfo -ComputerName $computer
.EXAMPLE
   Get-PartitionInfo -ComputerName (Get-Content -Path $path)
#>
function Get-PartitionInfo
{
    [CmdletBinding()]
    Param
    (
        # Computer Name
        # Can be used for multiple computers
        [Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true,Position=0)]
        [string[]]$ComputerName
    )

    Process
    {
        foreach($pc in $ComputerName)
        {

            Write-Verbose "Testing Connection to $pc"
            
        
            if(Test-Connection $pc -Count 1 -Quiet)
            {
                Try
                {
                    Write-Verbose "Gathering Data from $pc"
                    

                    $partitions = Get-WmiObject -Class Win32_logicaldisk -ComputerName $pc

                    #iterate through each element of each partition to create the custom object
                    foreach ($partition in $partitions)
                    {
                        foreach ($item in $partition)
                        {
                            if($partition.Size -gt 0)
                            {
                        
                                $part = @{

                                                    'Volume Name' = $partition.VolumeName
                                                    'Name' = $partition.Name
                                                    'Compressed' = $partition.Compressed
                                                    'Drive Type' = switch ($partition.DriveType)
                                                                                                        {
                                                                        0 {"Unknown"} 
                                                                        1 {"No Root Directory"}
                                                                        2 {"Removable Disk"} 
                                                                        3 {"Local Disk"} 
                                                                        4 {"Network Drive"} 
                                                                        5 {"Compact Disc"} 
                                                                        6 {"RAM Disk"}
                                                                    }
                                                    'Drive Size (GB)' = ($partition.Size /1GB -as [int])
                                                    'Free Space (GB)' = ($partition.FreeSpace /1GB -as [int])
                                                    'Free Space' = ("{0:P2}" -f ($partition.FreeSpace / $partition.Size))
                                                    'Volume Serial Number' = $partition.VolumeSerialNumber
                                                                                
                                               }

                            }
                            else
                            {
                                $part = @{

                                                    #'Volume Name' = $partition.VolumeName
                                                    'Name' = $partition.Name
                                                    #'Compressed' = $partition.Compressed
                                                    'Drive Type' = switch ($partition.DriveType)
                                                                    {
                                                                        0 {"Unknown"} 
                                                                        1 {"No Root Directory"}
                                                                        2 {"Removable Disk"} 
                                                                        3 {"Local Disk"} 
                                                                        4 {"Network Drive"} 
                                                                        5 {"Compact Disc"} 
                                                                        6 {"RAM Disk"}
                                                                    }
                                                    #'Drive Size (GB)' = ($partition.Size /1GB -as [int])
                                                    #'Free Space (GB)' = ($partition.FreeSpace /1GB -as [int])
                                                    #'Free Space (%)' = ("{0:P2}" -f ($partition.FreeSpace / $partition.Size) -as [int])
                                                    #'VolumeSerialNumber' = $partition.VolumeSerialNumber
                                                    }
                            }
                        }
                  
                        Write-Output (New-Object -TypeName PSObject -Property $part)
                  
                    }

                }
                Catch
                {
                    Write-Output "Machine not online or PowerShell is unable to read it"
                    Write-Output "$($Error[0])"
                }
            }
            else
            {
                Write-Output "Machine not online"
            }

            Write-Verbose "Report complete for $pc"
            
        }
    }

}


<#
.Synopsis
   Get Processor infromation for remote computer
.DESCRIPTION
   Retrieve Processor information from remote or local computer by using the WMI service.
.EXAMPLE
   Get-ProcessorInfo -ComputerName $pc
.EXAMPLE
   Get-ProcessorInfo -ComputerName (Get-Content -Path $path)
#>
function Get-ProcessorInfo
{
    [CmdletBinding()]
    Param
    (
        # Computer Name
        # Can be used for multiple computers
        [Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true,Position=0)]
        [string[]]$ComputerName
    )

    Process
    {
        foreach($pc in $ComputerName)
        {

            Write-Verbose "Testing Connection to $pc"
            
        
            if(Test-Connection $pc -Count 1 -Quiet)
            {
                Try
                {
                    Write-Verbose "Gathering Data from $pc"
                    
                
                    $processors = Get-WmiObject -Class win32_processor -ComputerName $pc

                    foreach ($processor in $processors)
                    {
                       $myobj = @{
                        
                            'Status' = $processor.Status
                            'Model' = $processor.Description
                            'Name' = $processor.Name
                            'Address Width' = $processor.AddressWidth
                            'Data Width' = $processor.DataWidth
                            'ExtClock' = $processor.ExtClock
                            'L2Cache Size (KB)' = $processor.L2CacheSize
                            'L3Cache Size (KB)' = $processor.L3CacheSize
                            'Max Clock Speed (MHz)' = $processor.MaxClockSpeed
                            'Current Voltage' = $processors.CurrentVoltage
                        
                       }
                   
                       Write-Output (New-Object -TypeName PSObject -Property $myobj ) 
                    }

                }
                Catch
                {
                    Write-Output "Machine $pc not online or PowerShell is unable to read it"
                    Write-Output "$($Error[0])"
                }
            }
            else
            {
                Write-Output "Machine not online"
            }

            Write-Verbose "Report complete for $pc"
            
        }
    }
}