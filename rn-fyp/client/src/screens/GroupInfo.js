//import liraries
import React, { Component } from 'react';
import { View, Text, StyleSheet, Image, AsyncStorage, ScrollView, SafeAreaView  } from 'react-native';
import { Header, Left, Right, Icon } from 'native-base';
import { theme } from '../core/theme';
import axios from 'axios';

// create a component
class GroupInfo extends Component {
    constructor(props)
    {
        super(props);

        this.state = {
            Supervisor_name: '',
            Cosupervisor_name: '',
            Project_title: '',
            Group_id: '',
            Leader_id: '',
            Group_members: [],
        }

        

        
    }
    static navigationOptions = {
        title: 'Group Info',
        drawerIcon : ({tintColor}) => (
            <Icon name="home" style = {{ fontSize: 24, color: theme.colors.primary }} />
        )
    }

    async getStudent() {
        try {
            const value = await AsyncStorage.getItem('User');
            if (value !== null) {
              // We have data!!
              axios.get(`http://192.168.1.2:80/api/student/GetByGroup/${value}`)
                .then(function(response) {
                    console.log(response.data)
                        
                    this.setState({
                        Supervisor_name: response.data[0].Supervisor_name,
                        Cosupervisor_name: response.data[0].Cosupervisor_name,
                        Project_title: response.data[0].Project_title,
                        Group_id: response.data[0].Group_id,
                        Leader_id: response.data[0].Leader_id,
                        Group_members: response.data[0].Student_list
                       
                    })

                    


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
            <SafeAreaView >
                <ScrollView >
                        <View style={styles.container}>
                            <Header style={{backgroundColor: theme.colors.primary}}>
                                <Left style={styles.marginZero}>
                                    <Icon name="menu" onPress={ () => this.props.navigation.openDrawer()} />
                                </Left>
                            </Header>
                            <View style={{flexDirection: 'column', alignItems: 'center', paddingVertical: 15}}>
                                <View >
                                    <Text style={{ fontSize: 20, fontWeight: 'bold', color: theme.colors.primary, }}>Group ID</Text>
                                </View>
                                <View>
                                    <Text style={{ fontSize: 20, fontStyle: 'italic'}}>{this.state.Group_id}</Text>
                                </View>
                            </View>

                            <View style={{flexDirection: 'column', alignItems: 'center', paddingVertical: 15}}>
                                <View >
                                    <Text style={{ fontSize: 20, fontWeight: 'bold', color: theme.colors.primary, }}>Leader ID</Text>
                                </View>
                                <View>
                                    <Text style={{ fontSize: 20, fontStyle: 'italic'}}>{this.state.Leader_id}</Text>
                                </View>
                            </View>


                            <View style={{flexDirection: 'column', alignItems: 'center', paddingVertical: 15}}>
                                <View >
                                    <Text style={{ fontSize: 20, fontWeight: 'bold', color: theme.colors.primary, }}>Supervisor Name</Text>
                                </View>
                                <View>
                                    <Text style={{ fontSize: 20, fontStyle: 'italic'}}>{this.state.Supervisor_name}</Text>
                                </View>
                            </View>

                            <View style={{flexDirection: 'column', alignItems: 'center', paddingVertical: 15}}>
                                <View >
                                    <Text style={{ fontSize: 20, fontWeight: 'bold', color: theme.colors.primary, }}>Co-Supervisor Name</Text>
                                </View>
                                <View>
                                    <Text style={{ fontSize: 20, fontStyle: 'italic'}}>{this.state.Cosupervisor_name == null ? '-' : this.state.Cosupervisor_name}</Text>
                                </View>
                            </View>

                            <View style={{flexDirection: 'column', alignItems: 'center', paddingVertical: 15}}>
                                <View >
                                    <Text style={{ fontSize: 20, fontWeight: 'bold', color: theme.colors.primary, }}>Project Title</Text>
                                </View>
                                <View>
                                    <Text style={{ fontSize: 20, fontStyle: 'italic'}}>{this.state.Project_title}</Text>
                                </View>
                            </View>

                           
                        </View>
                        </ScrollView>
                    </SafeAreaView>
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
export default GroupInfo;
