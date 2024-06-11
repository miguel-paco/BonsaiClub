import math
#import clr
#clr.AddReference("OpenCV.Net")
#from OpenCV.Net import Point2f
#from System import Single, Tuple
import numpy as np
#from keras.saving import load_model
import pandas as pd
#import pytorch_tabnet
#from pytorch_tabnet.tab_model import TabNetClassifier, TabNetRegressor
import xgboost as xgb

print("bEGIN")

# Global Variables
velocity = None
present_x = None
past_x = None
present_y = None
past_y = None
present_ori = None
past_ori = None
counter = None
history = None

def centdiff(signal):
    """
    CENTDIFF calculates the central difference of an input signal by approximating the derivative at each point using a combination of differences between neighboring elements.
    This can be useful in cases where you need to estimate the rate of change of the signal at different points.
    """
    # Calculate the first-order differences between adjacent elements of the input signal.
    # This is done using the numpy.diff() function in Python.
    # The result, diffSignal, will have one less element than the original signal, as it computes the difference between consecutive elements.
    #diffSignal = np.diff(signal, axis=0)
    #dx = 1
    #print(signal)
    diffSignal = np.gradient(signal)#, dx)
    print(diffSignal)
    # Calculate the central difference.
    # The central difference formula approximates the derivative at a point using the average of the differences on either side of that point.
    # The implementation involves adding the shifted versions of the diffSignal array, resulting in two arrays:
    #   [diffSignal; diffSignal[-1]]: This array is created by concatenating the original diffSignal with its last element. This is done to include the difference between the last and second-to-last elements in the computation.
    #   [diffSignal[0]; diffSignal]: This array is created by concatenating the first element of diffSignal with the original diffSignal. This is done to include the difference between the second and first elements in the computation.
    # Finally, the two modified arrays are added together, and the result is divided by 2 to compute the average. The outcome is assigned to the variable cdiff, which will contain the central difference values for each point in the input signal.
    #cdiff = (np.vstack((diffSignal, diffSignal[-1])) + np.vstack((diffSignal[0], diffSignal))) / 2 THIS IS THE OG LINE
    cdiff = diffSignal
    
    return cdiff



#@returns(float)
def calculate_velocities(value):
    global velocity, present_x, past_x, present_y, past_y, present_ori, past_ori #, counter, history
    #value = list(value)
    #centroid = [float(value.Item1), float(value.Item2)]
    #print(value)
    centroid_x = np.array(value.Centroid.X)#], [value.Centroid.Y]])
    if past_x is None:
        past_x = 0
    else: 
        past_x = present_x
    present_x = centroid_x
    #centroid_x_storage = []
    #for x_coord in centroid_x:
    #centroid_x_storage.append(centroid_x)
    #print(centroid_x_storage[-1])
    #print(centroid_x)
    centroid_y = np.array(value.Centroid.Y)
    if past_y is None:
        past_y = 0
    else: 
        past_y = present_y
    present_y = centroid_y
    #head = value.Item2 
    #diffCoord_x = centdiff(centroid_x) OG WAY
    #diffCoord_x = np.gradient(centroid_x)
    #print(past_x)
    diffCoord_x = (present_x - past_x)
    print(diffCoord_x)
    diffCoord_x = diffCoord_x * 60

    #print(diffCoord_x)
    diffCoord_y = (present_y - past_y)
    diffCoord_y = diffCoord_y * 60
    #orientation = angleline(centroid,head)
    print(diffCoord_y)
    ori = np.radians((value.Orientation))
    print(ori)
    if past_ori is None:
        past_ori = 0
    else: 
        past_ori = present_ori
    present_ori = ori
    print('computed ori radions')
    #ori = np.unwrap(ori)
    #ori = np.nan_to_num(ori, nan=np.nan, copy=True)
    #ori = np.interp(np.arange(len(ori)), np.where(~np.isnan(ori))[0], ori[~np.isnan(ori)])
    #VelForward_Mms = diffCoord[:, 0] * np.cos(ori) + diffCoord[:, 1] * np.sin(ori) THIS IS THE OG LINE
    VelForward_Mms = ((diffCoord_x * np.cos(ori)) + (diffCoord_y * np.sin(ori))-7.383423342219337)/5.924584445354852 #these numbers are the mean and std of the original dataset used for normalization
    print('computed vel')
    X = diffCoord_x#[:, 0]
    Y = diffCoord_y#[:, 1]
    diffOri = (present_ori - past_ori) 
    print(diffOri)
    VelAngular_Degs = ((np.degrees(diffOri)*60)-1.0271566491397963) / 202.38676608044608
    Walldist = ((55 - np.sqrt(np.multiply(X,X) + np.multiply(Y,Y)))-34.71692942696893) / 5.420970792185561
    #loaded_clf = TabNetClassifier()

    clf = xgb.XGBClassifier()
    booster = xgb.Booster()
    booster.load_model('C:/Users/Admin/Desktop/xgboost_100epochs_just_vel_othersave_sac.h5')
    clf._Booster = booster
    model = clf
    #model = xgb.XGBClassifier.load_model(fname = 'C:/Users/Admin/Desktop/xgboost_100epochs_just_vel_othersave_sac.h5')
    #model = loaded_clf
    print(model)
    #model = load_model('C:/Users/Admin/Desktop/tabnet_100epochs_just_vel_othersave.h5/network.pt') #loading model
    if math.isnan(VelForward_Mms):
        VelForward_Mms = 0

    if math.isnan(VelAngular_Degs):
        VelAngular_Degs = 0  

    if math.isnan(Walldist):
        Walldist = 0 
    dataframe_with_data = pd.DataFrame([[VelForward_Mms, VelAngular_Degs, Walldist]])
    print(dataframe_with_data)
    
    dataframe_with_data = dataframe_with_data.to_numpy()
    #dataframe_with_data = [[VelForward_Mms,VelAngular_Degs, Walldist]]
    #dataframe_with_data_present_store = []
    #while len(dataframe_with_data_present_store) < 30 :
    #    dataframe_with_data_present_store.append(dataframe_with_data) 
    #dataframe_with_data = np.reshape(dataframe_with_data, (-1,1, 3))
    
    y_pred = model.predict(dataframe_with_data)
    print(y_pred)
    output = pd.DataFrame(y_pred)
    print(output)
    output = output.iloc[0]
    
    return tuple(output)[0]


#model = load_model('/nfs/tank/chiappe/users/filipa.torrao/cluster_working_area/model_correct_velocities_250epochs_200bs_30psteps.h5') #loading model

def predict_behaviour(value):
    model = load_model('C:/Users/Admin/Desktop/model_just_velocities_CNN_250epochs_200bs_30psteps.h5') #loading model
    dataframe_with_data = pd.DataFrame[float(value.Item1),float(value.Item2), float(value.Item3)]
    y_pred = model.predict(dataframe_with_data,batch_size=1)#(a dataframe with the velocities and wall distance will go here)

    #export the y_pred to bonsai somehow

    y_pred_df = []
    y_pred_df.append(y_pred)
    return float(y_pred)

print("Reached the nend")