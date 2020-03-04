import React, { memo, Component } from 'React';
import { View, Platform, StyleSheet, SafeAreaView, ScrollView, Dimensions, Image, StatusBar, Text } from 'react-native';
import { createAppContainer } from 'react-navigation'
import { createDrawerNavigator, DrawerItems  } from 'react-navigation-drawer';
import { theme } from '../core/theme';
import Home from './Home';
import AllForms from './AllForms';
import Profile from './Profile';
import GroupInfo from './GroupInfo';
import { Icon } from 'react-native-elements';

const { width } = Dimensions.get('window')
const CustomDrawerComponent = (props) => {
    return (
        <SafeAreaView style={{ flex: 1}}>
            <View style={{ height: 150, backgroundColor: 'white', justifyContent: 'center', flexDirection: 'row'}}>
                <Image source={ require('../images/user.png')} style={{ height: 70, width: 70, borderRadius: 60,  marginLeft: -50, marginTop: 40,}} />
                <Text style={{ marginLeft:15, fontWeight: 'bold', fontSize: 15, marginTop: 65, }}>Muhammad Saad</Text>
            </View>
            <ScrollView>
                <DrawerItems {...props} />
            </ScrollView>
        </SafeAreaView>
    )
    
}

const MainNavigator = createAppContainer(createDrawerNavigator({
    First: AllForms,
    Second: Profile,
    Third: GroupInfo,
},
{
    contentComponent: CustomDrawerComponent,
    contentOptions: {
        activeTintColor: theme.colors.primary
    }
}));
class Dashboard extends Component {
    // componentDidMount() {
    //     axios.get('http://http://localhost:59782/api/form')
    // }

    render() {
        return (
            <View style={{ flex: 1, marginTop: StatusBar.currentHeight }}>          
                <MainNavigator />
            </View> 
        );
    }
}

const styles = StyleSheet.create({
    menu: {
      marginLeft: 50,
    },
    
  });

const App= createAppContainer(MainNavigator);

export default memo(Dashboard);
