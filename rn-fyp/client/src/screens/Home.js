//import liraries
import React, { Component } from 'react';
import { View, Text, StyleSheet  } from 'react-native';
import PureChart from 'react-native-pure-chart';
import { Header, Left, Right, Icon } from 'native-base';
import { theme } from '../core/theme';
import Dashboard from 'react-native-dashboard';

// create a component
class Home extends Component {

    constructor(props)
    {
        super(props);
       
    }
  
        static navigationOptions = {
            title: "Home",
            drawerIcon : ({tintColor}) => (
                <Icon name="home" style = {{ fontSize: 24, color: theme.colors.primary }} />
            )
        }
        render() {
            let formName = 'Mid Evaluation'
            let status = 'accepted';
            let marks = 9;
            const items = [
            { name: formName + '\n' + '  ' + status, background: status == 'accepted' ? '#02ef1d' : '#ef0202', icon: status == 'accepted' ? 'check' : 'times' },
            { name: formName + '\n' + '  ' + status, background: status == 'accepted' ? '#02ef1d' : '#ef0202', icon: 'percent' },
            
          ];
            return (
                <View style={styles.container}>
                    <Header style={{backgroundColor: theme.colors.primary}}>
                        <Left style={styles.marginZero}>
                            <Icon name="menu" onPress={ () => this.props.navigation.openDrawer()} />
                            <Text style={{ marginLeft: 40, marginTop: -30, fontSize: 20, fontWeight: 'bold'}}>Form Status</Text>
                        </Left>
                    </Header>

                    <Dashboard items={items} background={true} card={this._card} column={2} />
                    
                </View>
                      
        )
    }
}

// define your styles
const styles = StyleSheet.create({
    container: {
           flex: 1,   
      
    },

    marginZero: {
        marginLeft: -170
    }
});

//make this component available to the app
export default Home;
