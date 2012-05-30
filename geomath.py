'''
Created on May 18, 2012

@author: Eyal
'''

from math import radians, atan, tan, sin, cos, pi, sqrt, atan2, asin, log, degrees

def distance(lat1,lon1,lat2,lon2):
    if(lat1==lat2 and lon1==lon2):
        return(0)
    lon1 = radians(lon1); lat1 = radians(lat1)
    lon2 = radians(lon2); lat2 = radians(lat2)
    [a,b,f] = [6378137,6356752.3142,1/298.257223563]
    l = lon2 - lon1
    u1 = atan((1-f) * tan(lat1))
    u2 = atan((1-f) * tan(lat2))
    sin_u1 = sin(u1); cos_u1 = cos(u1)
    sin_u2 = sin(u2); cos_u2 = cos(u2)
    lambda_ = l
    lambda_pi = 2 * pi
    iter_limit = 20
    [cos_sq_alpha,sin_sigma,cos2sigma_m,cos_sigma,sigma] = [0,0,0,0,0]
    while(abs(lambda_-lambda_pi) > 1e-12 and --iter_limit>0):
        sin_lambda = sin(lambda_); cos_lambda = cos(lambda_)
        sin_sigma = sqrt((cos_u2*sin_lambda) * (cos_u2*sin_lambda) +
                         (cos_u1*sin_u2-sin_u1*cos_u2*cos_lambda) *
                         (cos_u1*sin_u2-sin_u1*cos_u2*cos_lambda))
        cos_sigma = sin_u1*sin_u2 + cos_u1*cos_u2*cos_lambda
        sigma = atan2(sin_sigma, cos_sigma)
        alpha = asin(cos_u1 * cos_u2 * sin_lambda / sin_sigma)
        cos_sq_alpha = cos(alpha) * cos(alpha)
        cos2sigma_m = cos_sigma - 2*sin_u1*sin_u2/cos_sq_alpha
        cc = f/16*cos_sq_alpha*(4+f*(4-3*cos_sq_alpha))
        lambda_pi = lambda_;
        lambda_ = (l + (1-cc) * f * sin(alpha) *
                  (sigma + cc*sin_sigma*(cos2sigma_m+cc*cos_sigma*(-1+2*cos2sigma_m*cos2sigma_m))))
    usq = cos_sq_alpha*(a*a-b*b)/(b*b)
    aa = 1 + usq/16384*(4096+usq*(-768+usq*(320-175*usq)))
    bb = usq/1024 * (256+usq*(-128+usq*(74-47*usq)))
    delta_sigma = bb*sin_sigma*(cos2sigma_m+bb/4*(cos_sigma*(-1+2*cos2sigma_m*cos2sigma_m)-
                  bb/6*cos2sigma_m*(-3+4*sin_sigma*sin_sigma)*(-3+4*cos2sigma_m*cos2sigma_m)))
    c = b*aa*(sigma-delta_sigma)
    return(c)

#from view-source:https://google-developers.appspot.com/maps/documentation/javascript/examples/map-coordinates
def lat_lon_zoom_to_point(lat, lon, zoom):
    x = 128 + lon * (256/360)
    siny = sin(radians(lat))
    if siny < -0.9999: siny = -0.9999
    if siny >  0.9999: siny =  0.9999
    y = 128 + 0.5 * log((1 + siny) / (1 - siny)) * -(256/(2 * pi))
    return(int(x * (1 << zoom)), int(y * (1 << zoom)))

#from http://williams.best.vwh.net/avform.htm#Intermediate
def intermediate_point(lat1, lon1, lat2, lon2, f):
    """
    Return the coordinate that is fraction f of the distance d between lat1,lon1 and lat2,lon2.
    lat1 and lat2 are from -90 to 90.  lon1 and lon2 are from -180 to 180
    """
    d = distance(lat1, lon1, lat2, lon2)
    lat1,lon1,lat2,lon2 = (radians(x) for x in (lat1,lon1,lat2,lon2))
    A=sin((1-f)*d)/sin(d)
    B=sin(f*d)/sin(d)
    x = A*cos(lat1)*cos(lon1) +  B*cos(lat2)*cos(lon2)
    y = A*cos(lat1)*sin(lon1) +  B*cos(lat2)*sin(lon2)
    z = A*sin(lat1)           +  B*sin(lat2)
    lat=atan2(z,sqrt(x*x+y*y))
    lon=atan2(y,x)
    lat, lon = tuple([degrees(x) for x in [lat,lon]])
    assert(lat > -90 and lat < 90)
    assert(lon > -180 and lon < 180)
    return(lat,lon)

if(__name__=='__main__'):
    print(intermediate_point(10,10,20,20,0.5))