//import liraries
import React, { Component } from 'react';
import { View, Text, ScrollView, StyleSheet } from 'react-native';
import { theme } from '../core/theme';
import { Header, Left, Right, Icon } from 'native-base';
import { DataTable } from 'react-native-paper';
import { Table, Row, Rows } from 'react-native-table-component';
import axios from 'axios';

// create a component
class AllForms extends Component {

    constructor(props)
    {
        super(props);
          
        this.state = {
            tableHead: ['Title', 'Status', 'id1', 'id2', 'id3' ],
            tableData: [
              ['Mid Evaluation', 'accepted', '10', '10','10'],
              ['Mid Evaluation', 'accepted', '10', '10','10'],
              ['Mid Evaluation', 'accepted', '10', '10','10'],
              ['Mid Evaluation', 'accepted', '10', '10','10'],
            ]
          }

    }
  
        static navigationOptions = {
            title: "View All Forms",
            drawerIcon : ({tintColor}) => (
                <Icon name="eye" style = {{ fontSize: 24, color: theme.colors.primary }} />
            )
        }

        async getForms() {
            try {
                const value = await AsyncStorage.getItem('Group_id');
                if (value !== null) {
                  // We have data!!
                  axios.get(`http://192.168.1.2:80/api/form/GetEvaluatedByGroupID/${value}`)
                    .then(function(response) {
                        this.setState({
                            Group_id: response.data[0].Form_status,
                            Student_email: response.data[0].Student_email,
                            Student_id: response.data[0].Student_id,
                            Student_name: response.data[0].Student_name,
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
            this.getForms();
        }
        render() {
            const state = this.state;
            return (
                <View style={styles.container}>
                    <Header style={{backgroundColor: theme.colors.primary}}>
                        <Left style={styles.marginZero}>
                            <Icon name="menu" onPress={ () => this.props.navigation.openDrawer()} />
                            <Text style={{ marginLeft: 40, marginTop: -30, fontSize: 20, fontWeight: 'bold'}}>All Forms</Text>
                        </Left>
                    </Header>

                    <Table borderStyle={{borderWidth: 2, borderColor: '#c8e1ff', marginTop: 30,}}>
                        <Row data={state.tableHead} style={styles.head} textStyle={styles.text}/>
                        <Rows data={state.tableData} textStyle={styles.text}/>
                    </Table>   
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
    },

    head: { height: 40, backgroundColor: '#f1f8ff' },
    text: { margin: 6 }

   
});

//make this component available to the app
export default AllForms;
