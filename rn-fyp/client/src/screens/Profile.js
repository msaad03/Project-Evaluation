//import liraries
import React, { Component } from 'react';
import { View, Text, StyleSheet, Image, AsyncStorage } from 'react-native';
import { Header, Left, Right, Icon } from 'native-base';
import { theme } from '../core/theme';
import axios from 'axios';

// create a component
class Profile extends Component {
    constructor(props)
    {
        super(props);

        this.state = {
            Group_id: '',
            Student_email: '',
            Student_id: '',
            Student_name: '',
        }

        

        
    }
    static navigationOptions = {
        title: 'Profile',
        drawerIcon : ({tintColor}) => (
            <Icon name="home" style = {{ fontSize: 24, color: theme.colors.primary }} />
        )
    }

    async getStudent() {
        try {
            const value = await AsyncStorage.getItem('User');
            if (value !== null) {
              // We have data!!
              axios.get(`http://192.168.1.2:80/api/student/get/${value}`)
                .then(function(response) {
                    this.setState({
                        Group_id: response.data[0].Group_id,
                        Student_email: response.data[0].Student_email,
                        Student_id: response.data[0].Student_id,
                        Student_name: response.data[0].Student_name,
                    })
                    console.log(response)
                }.bind(this))
                .catch(function(error) {
                    console.log(error)
                })
            }
          } catch (error) {
            // Error retrieving data from AsynStorage
          }
        
    }

    componentDidMount() {
        this.getStudent();
    }

    render() {
        return (
            <View style={styles.container}>
                    <Header style={{backgroundColor: theme.colors.primary}}>
                        <Left style={styles.marginZero}>
                            <Icon name="menu" onPress={ () => this.props.navigation.openDrawer()} />
                        </Left>
                    </Header>

                        <View style={styles.header}></View>
                        <Image style={styles.avatar} source={ require('../images/user1.png')} />

                        <View style={{flexDirection: 'column', alignItems: 'center', paddingVertical: 15}}>
                            <View >
                                <Text style={{ fontSize: 20, fontWeight: 'bold', color: theme.colors.primary, }}>ID</Text>
                            </View>
                            <View>
                                <Text style={{ fontSize: 20, fontStyle: 'italic'}}>{this.state.Student_id}</Text>
                            </View>
                        </View>

                        <View style={{flexDirection: 'column', alignItems: 'center', paddingVertical: 15}}>
                            <View >
                                <Text style={{ fontSize: 25, fontWeight: 'bold', color: theme.colors.primary, }}>Group Id</Text>
                            </View>
                            <View>
                                <Text style={{ fontSize: 20, fontStyle: 'italic'}}>{this.state.Group_id}</Text>
                            </View>
                        </View>

                        <View style={{flexDirection: 'column', alignItems: 'center', paddingVertical: 15}}>
                            <View >
                                <Text style={{ fontSize: 25, fontWeight: 'bold', color: theme.colors.primary, }}>Name</Text>
                            </View>
                            <View>
                                <Text style={{ fontSize: 20, fontStyle: 'italic'}}>{this.state.Student_name}</Text>
                            </View>
                        </View>

                        <View style={{flexDirection: 'column', alignItems: 'center', paddingVertical: 15}}>
                            <View >
                                <Text style={{ fontSize: 25, fontWeight: 'bold', color: theme.colors.primary, }}>Email</Text>
                            </View>
                            <View>
                                <Text style={{ fontSize: 20, fontStyle: 'italic'}}>{this.state.Student_email}</Text>
                            </View>
                        </View>

            </View>
        );
    }
}

// define your styles
const styles = StyleSheet.create({
    container: {
        flex: 1,
        
    },

    marginZero: {
        marginLeft: -170,
    },

    header:{
        backgroundColor: "#00BFFF",
        height:200,
      },
      avatar: {
        width: 130,
        height: 130,
        borderRadius: 63,
        borderWidth: 4,
        borderColor: "white",
        marginBottom:10,
        alignSelf:'center',
        position: 'absolute',
        marginTop:130
      },
      name:{
        fontSize:22,
        color:"#FFFFFF",
        fontWeight:'600',
      },
      body:{
        marginTop:40,
      },
      bodyContent: {
        flex: 1,
        alignItems: 'center',
        padding:30,
      },
      name:{
        fontSize:28,
        color: "#696969",
        fontWeight: "600"
      },
      info:{
        fontSize:16,
        color: "#00BFFF",
        marginTop:10
      },
      description:{
        fontSize:16,
        color: "#696969",
        marginTop:10,
        textAlign: 'center'
      },
      buttonContainer: {
        marginTop:10,
        height:45,
        flexDirection: 'row',
        justifyContent: 'center',
        alignItems: 'center',
        marginBottom:20,
        width:250,
        borderRadius:30,
        backgroundColor: "#00BFFF",
      },

      left: {
        color: '#A8A8A8',
        fontSize: 20,
        marginLeft: 50,
        marginTop: 25,
        marginBottom: 15,
 
    },
    right: {
        color: '#000000',
        fontSize: 20,
        marginLeft: 20,
        marginTop: 25,
        marginBottom: 15,

       
    },

    
});

//make this component available to the app
export default Profile;
