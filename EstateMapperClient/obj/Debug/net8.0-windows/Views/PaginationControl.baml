   M S B A M L   `   `   `  ����M  IEstateMapperClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:EstateMapperClient.ViewsEstateMapperClient.Views  5      0    *EstateMapperClient.Views.PaginationControl   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      8local&clr-namespace:EstateMapperClient.Views     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      .��5      �� $��Center=�6     �� Orientation$  
Horizontal=�6.   .+�5   
   �� $��«��5       &  "MaterialDesignIconForegroundButton#��[  5       � ToolTip$( "MaterialDesignIconForegroundButton��5      ��5      �� *FirstPageCommand+5      �� 5   
   $	��‹��5      #��[  5      $( "MaterialDesignIconForegroundButton��5      ��5      �� *PreviousPageCommand+5      �� 5   
   ��6   �� *PageNumbers+��5      �� 5      .R�5      �� $  
Horizontal=�6!   5      ��5      �� 5       .R�5!      �� ._�5#      ��  ��Name$ 
PageButton��5$         MaterialDesignFlatDarkButton#��[ 5(      ��5%      �� *DataContext.GoToPageCommand+ �� RelativeSource ��  �� AncestorType ItemsControl��5&      �� ��5'      �� 5(      5)      5*      5,      �� 5/   
   $	��›��51      #��[  52      $( "MaterialDesignIconForegroundButton��53      ��50      �� *NextPageCommand+53      �� 54   
   $��»��56      #��[  57      $( "MaterialDesignIconForegroundButton��58      ��55      �� *LastPageCommand+58      �� 5;   
   $
��10,0q�6   $��Center=�6"   �� 5<      $	�第��6   �� 6   �6"   �� *CurrentPage+ �� 5=      $�页/共��6   �� 6    �6$   �� *
TotalPages+�� 6C   $	�页��6G   5>      �� 5A   
   $��50��5B      $
��10,0q�5C      ��5D      �� *JumpPageNumber+�� 5E   
   $��跳转��5G        MaterialDesignFlatButton#��[ 5H      $" MaterialDesignOutlinedButton��5I      ��5F      �� *JumpPageCommand+5I      5J      5K      