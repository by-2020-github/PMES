﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PMES.Manual.Net6.ViewModels;

namespace PMES.Manual.Net6.Views
{
    /// <summary>
    /// MainView.xaml 的交互逻辑
    /// </summary>
    public partial class MainView : Window
    {
        private readonly MainViewModel _viewModel = new MainViewModel();
        public MainView()
        {
            InitializeComponent();
            this.DataContext = _viewModel;
        }
    }
}
