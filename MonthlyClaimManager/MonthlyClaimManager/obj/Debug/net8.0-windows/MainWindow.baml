   M S B A M L   `   `   `  ����N  JMonthlyClaimManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:MonthlyClaimManagerMonthlyClaimManager  5   	   $    MonthlyClaimManager.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   3local!clr-namespace:MonthlyClaimManager     5   	     � Title$  
MainWindow��5   	   $	��450��6   $	��800��6)   .��5	      � .+�5
   
   �� .@�5      �� $��Lecturer Dashboard��6   .��5      �� .+�5      �� $��Submit Monthly Claim��6    $��20(�6<   $
��Bold"�6J   $��10q�6\   �� 5       �� Orientation$ 
Horizontal=�6!   $��10q�6:   .+�5      �� $��Lecturer ID:��6$   $��Center=�68   $	��100��6S   �� 5      -   5        $��LecturerIDTextBox��5   "   $	��200��6;   5      �� 5      $ 
Horizontal=�6!   $��10q�6:   .+�5      �� $��Lecturer Name:��6$   $��Center=�6:   $	��100��6U   �� 5      -   5        $��LecturerNameTextBox��5   "   $	��200��6=   5      �� 5      $ 
Horizontal=�6!   $��10q�6:   .+�5      �� $��Hours Worked:��6$   $��Center=�69   $	��100��6T   �� 5      -   5        $��HoursWorkedTextBox��5   "   $	��200��6<   5      �� 5       $ 
Horizontal=�6!   $��10q�6:   .+�5!      �� $��Hourly Rate:��6$   $��Center=�68   $	��100��6S   �� 5"      -   5        $��HourlyRateTextBox��5"   "   $	��200��6;   5#      �� 5&      $ 
Horizontal=�6!   $��10q�6:   .+�5'      �� $!��Upload Supporting Document:��6$   $��Center=�6G   $	��200��6b   �� 5(      -   5        $��UploadDocumentButton��5(   ;   $��Upload Document��6!   $	��150��6t   5)      �� 5,      -   5        $��Submit Claim��5,      $	��150��6N   $��10q�6Z   �� 5/      $��Your Claims Status��6    $��16(�6:   $
��Bold"�6H   $��10q�6Z   &   System.Windows.Controls.DataGrid  50      -   5        $��LecturerClaimsDataGrid��50         AutoGenerateColumns$
 True��6=   $	��200��6X   51      52      �� 55      $#��Coordinator/Manager Dashboard��6   .��56      �� .+�57      �� $��Verify and Approve Claims��6    $��20(�6A   $
��Bold"�6O   $��10q�6a   �� 5:      $��Pending Claims��6    $��16(�66   $
��Bold"�6D   $��10q�6V   �� 5;      -   5        $��PendingClaimsListBox��5;      $	��200��6:   6G   �� 5>      $��Claim Details��6    $��16(�65   $
��Bold"�6C   $��10q�6U   �� 5?      $ 
Horizontal=�6!   $��10q�6:   .+�5@      �� $��Lecturer ID:��6$   $��Center=�68   $	��100��6S   �� 5A      -	   5        $��ClaimLecturerIDTextBox��5A   "    �� 
IsReadOnly$
 True��6@   $	��200��6R   5B      �� 5D      $ 
Horizontal=�6!   $��10q�6:   .+�5E      �� $��Lecturer Name:��6$   $��Center=�6:   $	��100��6U   �� 5F      -
   5        $��ClaimLecturerNameTextBox��5F   "   $
 True��6B   $	��200��6T   5G      �� 5I      $ 
Horizontal=�6!   $��10q�6:   .+�5J      �� $��Hours Worked:��6$   $��Center=�69   $	��100��6T   �� 5K      -   5        $��ClaimHoursWorkedTextBox��5K   "   $
 True��6A   $	��200��6S   5L      �� 5N      $ 
Horizontal=�6!   $��10q�6:   .+�5O      �� $��Claim Amount:��6$   $��Center=�69   $	��100��6T   �� 5P      -   5        $��ClaimAmountTextBox��5P   "   $
 True��6<   $	��200��6N   5Q      �� 5S      $ 
Horizontal=�6!   $��10q�6:   .+�5T      �� $��Claim Status:��6$   $��Center=�69   $	��100��6T   �� 5U      -   5        $��ClaimStatusTextBox��5U   "   $
 True��6<   $	��200��6N   5V      �� 5Y      $ 
Horizontal=�6!   $
��Left=�6:   $��10q�6U   .+�5Z      �� -   5        $��Approve Claim��5Z   !   $	��150��69   $��5q�6`   �� 5[      -   5        $��Reject Claim��5[   !   $	��150��68   $��5q�6^   5\      5]      5^      5_      5`      5a      